using Microsoft.Data.SqlClient;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Query;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;
using System.Data;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class NewsRepository : INewsRepository
    {
        private readonly IExecuteQuery execute;

        public NewsRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<object>> GetNewsAsync(NewsQuery query, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetAllNews", CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@NewsCategoryId", (object)query.NewsCategoryId! ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PageNumber", query.PageNumber);
                cmd.Parameters.AddWithValue("@PageSize", query.PageSize);

                var rdr = await execute.ExecuteReaderAsync(cmd);
                var data = new List<NewsDto>();

                if (rdr != null)
                {
                    do
                    {
                        data.Add(new NewsDto()
                        {
                            TopicId = rdr.SafeGetInt("TopicId"),
                            NewsLabel = rdr.SafeGetString("Label"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            TopicTag = rdr.SafeGetString("TopicTag"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            IsVideo = rdr.SafeGetBoolean("IsVideo"),
                            IsArrows = rdr.SafeGetBoolean("IsArrows"),
                            IsViewMore = rdr.SafeGetBoolean("IsViewMore"),
                            Action = rdr.SafeGetString("Action"),
                            ButtonActionTitle = rdr.SafeGetString("ButtonActionTitle"),
                            TotalNews = rdr.SafeGetInt("TotalNews"),
                            CategoryId = rdr.SafeGetInt("CategoryId")
                        });
                    }while(await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                if (query.NewsCategoryId.HasValue && query.NewsCategoryId > 0)
                {
                    return new Response<object>(200, "Success", data, true);
                }

                var contents = data
                    .GroupBy(m => new
                    {
                        m.NewsLabel,
                        m.IsArrows,
                        m.IsViewMore,
                        m.Action,
                        m.ButtonActionTitle,
                        m.CategoryId
                    })
                    .Select(g => new GroupedNewsListDto
                    {
                        NewsLabel = g.Key.NewsLabel,
                        IsArrows = g.Key.IsArrows,
                        IsViewMore = g.Key.IsViewMore,
                        Action = g.Key.Action,
                        ButtonActionTitle = g.Key.ButtonActionTitle,
                        TotalNews = g.First().TotalNews,
                        CategoryId = g.Key.CategoryId,
                        News = g.ToList()
                    });

                return new Response<object>(200, "Success", contents, true);
            }
            catch (SqlException ex)
            {
                return new Response<object>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<object>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }
    
    }
}
