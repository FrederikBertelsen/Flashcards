using Backend.Exceptions;
using Backend.Models;
using Backend.Models.DTOs;
using Backend.Models.DTOs.New;
using Backend.Repositories;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/learning-sessions")]
[ApiController]
public class LearningSessionController(LearningSessionRepository learningSessionRepository, SessionManager sessionManager)
    : BaseController(sessionManager)
{
    [HttpPost]
    public async Task<ActionResult<LearningSessionDTO>> CreateLearningSession(NewLearningSessionDTO newLearningSessionDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await learningSessionRepository.CreateLearningSession(newLearningSessionDto, user);
                return NoContent();
            });
        });
    }
    
    [HttpGet("/api/users/{userId}/learning-sessions")]
    public async Task<ActionResult<IEnumerable<LearningSessionDTO>>> GetLearningSessionsByUserId(string userId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                var learningSessions = await learningSessionRepository.GetLearningSessionsByUserId(userId, user);
                return Ok(learningSessions);
            });
        });
    }

    [HttpGet("{learningSessionId}/due-reviews")]
    public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetDueReviewsByLearningSessionId(string learningSessionId)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                var dueReviews = await learningSessionRepository.GetDueReviewsByLearningSessionId(learningSessionId, user);
                return Ok(dueReviews);
            });
        });
    }

    [HttpPost("/api/reviews")]
    public async Task<IActionResult> CreateReview([FromBody] NewReviewDto newReviewDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await learningSessionRepository.CreateReview(newReviewDto, user);
                return NoContent();
            });
        });
    }

    [HttpPut("/api/reviews/{reviewId}")]
    public async Task<IActionResult> UpdateReview(string reviewId,[FromBody] Card card)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await learningSessionRepository.UpdateReview(reviewId, card, user);
                return NoContent();
            });
        });
    }

    [HttpPost("/api/reviews-logs")]
    public async Task<IActionResult> CreateReviewLog([FromBody] NewReviewLogDTO newReviewLogDto)
    {
        return await ExceptionHandler.HandleAsync(async () =>
        {
            return await WithAuthAsync(async user =>
            {
                await learningSessionRepository.CreateReviewLog(newReviewLogDto, user);
                return NoContent();
            });
        });
    }
}