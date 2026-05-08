using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class VideoRepository : IVideoRepository
    {
        private readonly IExecuteQuery execute;

        public VideoRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<IEnumerable<GroupedVideosDto>>> GetLatestVideosAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetLatestVideo" };
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var data = new List<VideoDto>();

                if (rdr != null)
                {
                    do
                    {
                        data.Add(new VideoDto()
                        {
                            VideoId = rdr.SafeGetInt("VideoId"),
                            VideoLabel = rdr.SafeGetString("VideoLabel"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            VideoTag = rdr.SafeGetString("VideoTag"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            IsReference = rdr.SafeGetBoolean("IsReference"),
                            IsStory = rdr.SafeGetBoolean("IsStory"),
                            IsVideoSeries = rdr.SafeGetBoolean("IsVideoSeries"),
                            IsTheArchive = rdr.SafeGetBoolean("IsTheArchive"),
                            Action = rdr.SafeGetString("Action"),
                            ButtonActionTitle = rdr.SafeGetString("ButtonActionTitle")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                var contents = data
                    .GroupBy(m => new
                    {
                        m.VideoLabel,
                        m.IsStory,
                        m.IsVideoSeries,
                        m.IsTheArchive,
                        m.Action,
                        m.ButtonActionTitle,
                    })
                    .Select(g => new GroupedVideosDto
                    {
                        VideoLabel = g.Key.VideoLabel,
                        IsStory = g.Key.IsStory,
                        IsVideoSeries = g.Key.IsVideoSeries,
                        IsTheArchive = g.Key.IsTheArchive,
                        Action = g.Key.Action,
                        ButtonActionTitle = g.Key.ButtonActionTitle,
                        Videos = g.ToList()
                    });

                return new Response<IEnumerable<GroupedVideosDto>>(200, "Success", contents, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedVideosDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedVideosDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<GroupedVideosDto>>> GetSeriesAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetSeries" };
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var data = new List<VideoDto>();

                if (rdr != null)
                {
                    do
                    {
                        data.Add(new VideoDto()
                        {
                            VideoId = rdr.SafeGetInt("VideoId"),
                            VideoLabel = rdr.SafeGetString("VideoLabel"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            VideoTag = rdr.SafeGetString("VideoTag"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            IsReference = rdr.SafeGetBoolean("IsReference"),
                            IsStory = rdr.SafeGetBoolean("IsStory"),
                            IsVideoSeries = rdr.SafeGetBoolean("IsVideoSeries")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                var contents = data
                    .GroupBy(m => new
                    {
                        m.VideoLabel,
                        m.IsStory,
                        m.IsVideoSeries
                    })
                    .Select(g => new GroupedVideosDto
                    {
                        VideoLabel = g.Key.VideoLabel,
                        IsStory = g.Key.IsStory,
                        IsVideoSeries = g.Key.IsVideoSeries,
                        Videos = g.ToList()
                    });

                return new Response<IEnumerable<GroupedVideosDto>>(200, "Success", contents, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedVideosDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedVideosDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<GroupedVideosDto>>> GetTheArchiveAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetTheArchive" };
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var data = new List<VideoDto>();

                if (rdr != null)
                {
                    do
                    {
                        data.Add(new VideoDto()
                        {
                            VideoId = rdr.SafeGetInt("VideoId"),
                            VideoLabel = rdr.SafeGetString("VideoLabel"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            VideoTag = rdr.SafeGetString("VideoTag"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            IsReference = rdr.SafeGetBoolean("IsReference"),
                            IsStory = rdr.SafeGetBoolean("IsStory"),
                            IsVideoSeries = rdr.SafeGetBoolean("IsVideoSeries"),
                            IsTheArchive = rdr.SafeGetBoolean("IsTheArchive"),
                            Action = rdr.SafeGetString("Action"),
                            ButtonActionTitle = rdr.SafeGetString("ButtonActionTitle")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                var contents = data
                    .GroupBy(m => new
                    {
                        m.VideoLabel,
                        m.IsStory,
                        m.IsVideoSeries,
                        m.IsTheArchive,
                        m.Action,
                        m.ButtonActionTitle
                    })
                    .Select(g => new GroupedVideosDto
                    {
                        VideoLabel = g.Key.VideoLabel,
                        IsStory = g.Key.IsStory,
                        IsVideoSeries = g.Key.IsVideoSeries,
                        IsTheArchive = g.Key.IsTheArchive,
                        Action = g.Key.Action,
                        ButtonActionTitle = g.Key.ButtonActionTitle,
                        Videos = g.ToList()
                    });

                return new Response<IEnumerable<GroupedVideosDto>>(200, "Success", contents, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedVideosDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedVideosDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<HomeVideoDto>>> GetTheAllVideosAsync(int page = 1, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetAllVideos" };
                cmd.Parameters.AddWithValue("@Page", page);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var values = new List<HomeVideoDto>();

                if (rdr != null)
                {
                    do
                    {
                        values.Add(new HomeVideoDto()
                        {
                            VideoId = rdr.SafeGetInt("VideoId"),
                            Title = rdr.SafeGetString("Title"),
                            VideoTag = rdr.SafeGetString("VideoTag"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            VideoUrl = rdr.SafeGetString("VideoUrl")
                        });
                    }
                    while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }


                return new Response<IEnumerable<HomeVideoDto>>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeVideoDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeVideoDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<int>> GetTheAllVideosCountAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetAllVideosCount" };
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var values = 0;

                if (rdr != null)
                {
                    values = rdr.SafeGetInt("Count");
                }

                return new Response<int>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<int>(400, "Database Error: " + ex.Message, 0!, false);
            }
            catch (Exception ex)
            {
                return new Response<int>(500, $"Internal Server Error {ex.Message}", 0!, false);
            }
        }
    }
}
