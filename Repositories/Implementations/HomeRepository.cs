using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class HomeRepository : IHomeRepository
    {
        private readonly IExecuteQuery execute;

        public HomeRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<IEnumerable<HomeClubNewsDto>>> GetHomeClubNewsAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetHomeClubNews";
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var clubNews = new List<HomeClubNewsDto>();
                if(rdr is not null)
                {
                    do
                    {
                        clubNews.Add(new HomeClubNewsDto()
                        {
                            NewsId = rdr.SafeGetInt("NewsId"),
                            Title = rdr.SafeGetString("Title"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            ClubCrest = rdr.SafeGetString("ClubCrest")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<HomeClubNewsDto>>(200, "Success", clubNews, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeClubNewsDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeClubNewsDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<GroupedHomeMatchesDto>>> GetHomeMatchesAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetHomeMatches" };
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var matches = new List<HomeMatchesDto>();

                if (rdr != null)
                {
                    while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                    {
                        matches.Add(new HomeMatchesDto()
                        {
                            MatchId = rdr.SafeGetInt("MatchId"),
                            MatchDate = rdr.SafeGetString("MatchDate"),
                            kickoffTime = rdr.SafeGetString("kickoffTime"),
                            HomeClubName = rdr.SafeGetString("HomeClubName"),
                            AwayClubName = rdr.SafeGetString("AwayClubName"),
                            HomeClubCrest = rdr.SafeGetString("HomeClubCrest"),
                            AwayClubCrest = rdr.SafeGetString("AwayClubCrest"),
                            HomeClubTheme = rdr.SafeGetString("HomeClubTheme"),
                            AwayClubTheme = rdr.SafeGetString("AwayClubTheme"),
                            HomeClubGoal = rdr.SafeGetString("HomeClubGoal"),
                            AwayClubGoal = rdr.SafeGetString("AwayClubGoal"),
                            KickoffStatus = rdr.SafeGetString("KickoffStatus"),
                            IsGameFinished = rdr.SafeGetString("IsGameFinished"),
                        });
                    }
                }

                var groupedMatches = matches
                    .GroupBy(m => m.MatchDate)
                    .Select(g => new GroupedHomeMatchesDto
                    {
                        MatchDate = g.Key,
                        Matches = g.ToList()
                    });

                return new Response<IEnumerable<GroupedHomeMatchesDto>>(200, "Success", groupedMatches, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedHomeMatchesDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedHomeMatchesDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomeNewsAsTopicAsync(List<string> newsTags, int pageSize = 5, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetNewsAsTopic";
                cmd.Parameters.AddWithValue("@NewsTagJson", JsonConvert.SerializeObject(newsTags));
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var news = new List<HomeNewsTopicDto>();

                if (rdr != null)
                {
                    do
                    {
                        news.Add(new HomeNewsTopicDto()
                        {
                            TopicId = rdr.SafeGetInt("TopicId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            TopicTag = rdr.SafeGetString("TopicTag"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            IsVideo = rdr.SafeGetBoolean("IsVideo"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }


                return new Response<IEnumerable<HomeNewsTopicDto>>(200, "Success", news, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            };
        }

        public async Task<Response<IEnumerable<RelatedDto>>> GetHomeNewsAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetHomeNews";
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var news = new List<RelatedDto>();
                if (rdr is not null)
                {
                    do
                    {
                        news.Add(new RelatedDto
                        {
                            RelatedId = rdr.SafeGetInt("RelatedId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            TagName = rdr.SafeGetString("TagName"),
                            IsVideo = rdr.SafeGetBoolean("IsVideo")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<RelatedDto>>(200, "Success", news, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<RelatedDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<RelatedDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<GroupedNewsDto>>> GetHomeNewsAsync(List<string> newsTags, int pageSize = 5, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetNews";
                cmd.Parameters.AddWithValue("@NewsTagJson", JsonConvert.SerializeObject(newsTags));
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var news = new List<HomeNewsDto>();

                if (rdr != null)
                {
                    do
                    {
                        news.Add(new HomeNewsDto()
                        {
                            NewsId = rdr.SafeGetInt("NewsId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Image"),
                            TagName = rdr.SafeGetString("TagName")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                var groupedStoriesNews = news
                    .GroupBy(m => m.TagName)
                    .Select(g => new GroupedNewsDto
                    {
                        NewsLabel = g.Key,
                        News = g.ToList()
                    });

                return new Response<IEnumerable<GroupedNewsDto>>(200, "Success", groupedStoriesNews, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedNewsDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedNewsDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<HomeNewsDto>>> GetHomeNewsFromTheClubsAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiNewsFromTheClubs";
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var news = new List<HomeNewsDto>();

                if (rdr != null)
                {
                    do
                    {
                        news.Add(new HomeNewsDto()
                        {
                            NewsId = rdr.SafeGetInt("NewsId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            TagName = rdr.SafeGetString("TagName"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }


                return new Response<IEnumerable<HomeNewsDto>>(200, "Success", news, true);
            }
            catch (SqlException ex) 
            { 
                return new Response<IEnumerable<HomeNewsDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeNewsDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomeNewsMulitTopicAsync(List<string> newsTags, int pageSize = 5, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetMultipleNews";
                cmd.Parameters.AddWithValue("@NewsTagJson", JsonConvert.SerializeObject(newsTags));
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var news = new List<HomeNewsTopicDto>();

                if (rdr != null)
                {
                    do
                    {
                        news.Add(new HomeNewsTopicDto()
                        {
                            TopicId = rdr.SafeGetInt("TopicId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            TopicTag = rdr.SafeGetString("TopicTag"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            IsVideo = rdr.SafeGetBoolean("IsVideo"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }


                return new Response<IEnumerable<HomeNewsTopicDto>>(200, "Success", news, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<NewsRelatedGroupDto>> GetHomeNewsViewerAsync(int newsId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand
                {
                    CommandText = "PL_ApiGetNewsViewer"
                };
                cmd.Parameters.AddWithValue("@NewsId", newsId);

                using var rdr = await execute.ExecuteReaderAsync(cmd, ct);

                if (rdr is null)
                {
                    return new Response<NewsRelatedGroupDto>(404, "News not found", null!, false);
                }
                else
                {
                    var related = new NewsRelatedGroupDto();

                    related.News.NewsId = rdr.SafeGetInt("NewsId");
                    related.News.Title = rdr.SafeGetString("Title");
                    related.News.Subtitle = rdr.SafeGetString("Subtitle");
                    related.News.Description = rdr.SafeGetString("Description");
                    related.News.NewsTag = rdr.SafeGetString("NewsTag");
                    related.News.Thumbnail = rdr.SafeGetString("Thumbnail");
                    related.News.VideoUrl = rdr.SafeGetString("VideoUrl");
                    related.News.PublishedDate = rdr.SafeGetString("PublishedDate");
                    related.News.IsVideo = rdr.SafeGetBoolean("IsVideo");

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                        {
                            related.Relateds.Add(new RelatedDto
                            {
                                RelatedId = rdr.SafeGetInt("RelatedId"),
                                Title = rdr.SafeGetString("Title"),
                                Thumbnail = rdr.SafeGetString("Thumbnail"),
                                ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                                TagName = rdr.SafeGetString("TagName"),
                                IsVideo = rdr.SafeGetBoolean("IsVideo")
                            });
                        }
                    }
                    return new Response<NewsRelatedGroupDto>(200, "Success", related, true);
                }
            }
            catch (SqlException ex)
            {
                return new Response<NewsRelatedGroupDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<NewsRelatedGroupDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomePremierLeagueNewOnlyAsync(int pageSize = 5, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiPremierLeagueNewsOnly";
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var news = new List<HomeNewsTopicDto>();

                if (rdr != null)
                {
                    do
                    {
                        news.Add(new HomeNewsTopicDto()
                        {
                            TopicId = rdr.SafeGetInt("TopicId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            TopicTag = rdr.SafeGetString("TopicTag"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            IsVideo = rdr.SafeGetBoolean("IsVideo"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }


                return new Response<IEnumerable<HomeNewsTopicDto>>(200, "Success", news, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            };
        }

        public async Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomeQuizzesNewOnlyAsync(int pageSize = 5, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetQuizzeNewsOnly";
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var news = new List<HomeNewsTopicDto>();

                if (rdr != null)
                {
                    do
                    {
                        news.Add(new HomeNewsTopicDto()
                        {
                            TopicId = rdr.SafeGetInt("TopicId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            TopicTag = rdr.SafeGetString("TopicTag"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            IsVideo = rdr.SafeGetBoolean("IsVideo"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }


                return new Response<IEnumerable<HomeNewsTopicDto>>(200, "Success", news, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
            ;
        }

        public async Task<Response<IEnumerable<GroupedVideoDto>>> GetHomeStoriesNewsAsync(List<string> videosTag, int pageSize = 10, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetHomeStoriesNews" };
                cmd.Parameters.AddWithValue("@VideosTagJson", JsonConvert.SerializeObject(videosTag));
                cmd.Parameters.AddWithValue("@PageSize", pageSize);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var storiesNews = new List<HomeVideoDto>();

                if (rdr != null)
                {
                    do
                    {
                        storiesNews.Add(new HomeVideoDto()
                        {
                            VideoId = rdr.SafeGetInt("VideoId"),
                            Title = rdr.SafeGetString("Title"),
                            VideoTag = rdr.SafeGetString("VideoTag"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            VideoUrl = rdr.SafeGetString("VideoUrl")
                        });
                    }
                    while (await rdr.ReadAsync(ct).ConfigureAwait(false)) ;
                }

                var groupedStoriesNews = storiesNews
                    .GroupBy(m => m.VideoTag)
                    .Select(g => new GroupedVideoDto
                    {
                        VideoLabel = g.Key,
                        Videos = g.ToList()
                    });

                return new Response<IEnumerable<GroupedVideoDto>>(200, "Success", groupedStoriesNews, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedVideoDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedVideoDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<GroupedVideoDto>>> GetHomeVideosAsync(List<string> videoCategories, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetHomeVideos";
                cmd.Parameters.AddWithValue("@VideoCategoryJson", JsonConvert.SerializeObject(videoCategories));
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var videos = new List<HomeVideoDto>();

                if (rdr != null)
                {
                    while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                    {
                        videos.Add(new HomeVideoDto()
                        {
                            VideoId = rdr.SafeGetInt("VideoId"),
                            Title = rdr.SafeGetString("Title"),
                            VideoTag = rdr.SafeGetString("VideoTag"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            VideoUrl = rdr.SafeGetString("VideoUrl")
                        });
                    }
                }

                var groupedStoriesNews = videos
                    .GroupBy(m => m.VideoTag)
                    .Select(g => new GroupedVideoDto
                    {
                        VideoLabel = g.Key,
                        Videos = g.ToList()
                    });

                return new Response<IEnumerable<GroupedVideoDto>>(200, "Success", groupedStoriesNews, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedVideoDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedVideoDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<VideoRelatedGroupDto>> GetHomeVideoViewerAsync(int videoId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetVideoViewer";
                cmd.Parameters.AddWithValue("@VideoId", videoId);
                var rdr = await execute.ExecuteReaderAsync(cmd);

                if (rdr is null)
                {
                    return new Response<VideoRelatedGroupDto>(404, "Video not found", null!, false);
                }
                else
                {
                    var related = new VideoRelatedGroupDto();

                    related.Video.VideoId = rdr.SafeGetInt("VideoId");
                    related.Video.Title = rdr.SafeGetString("Title");
                    related.Video.Description = rdr.SafeGetString("Description");
                    related.Video.VideoTag = rdr.SafeGetString("VideoTag");
                    related.Video.Thumbnail = rdr.SafeGetString("Thumbnail");
                    related.Video.VideoUrl = rdr.SafeGetString("VideoUrl");
                    related.Video.PublishedDate = rdr.SafeGetString("PublishedDate");

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                        {
                            related.Relateds.Add(new RelatedDto
                            {
                                RelatedId = rdr.SafeGetInt("RelatedId"),
                                Title = rdr.SafeGetString("Title"),
                                Thumbnail = rdr.SafeGetString("Thumbnail"),
                                ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                                TagName = rdr.SafeGetString("TagName"),
                                IsVideo = rdr.SafeGetBoolean("IsVideo")
                            });
                        }
                    }
                    return new Response<VideoRelatedGroupDto>(200, "Success", related, true);
                }
            }
            catch (SqlException ex)
            {
                return new Response<VideoRelatedGroupDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<VideoRelatedGroupDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            };
        }
    }
}
