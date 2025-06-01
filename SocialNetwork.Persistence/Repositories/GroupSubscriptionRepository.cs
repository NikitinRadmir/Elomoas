using Elomoas.Application.Interfaces.Repositories;
using Elomoas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace Elomoas.Persistence.Repositories
{
    public class GroupSubscriptionRepository : IGroupSubscriptionRepository
    {
        private readonly IGenericRepository<GroupSubscription> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public GroupSubscriptionRepository(
            IGenericRepository<GroupSubscription> repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsSubscribed(int userId, int groupId)
        {
            return await _repository.Entities
                .AnyAsync(x => x.UserId == userId && x.GroupId == groupId);
        }

        public async Task Subscribe(int userId, int groupId)
        {
            if (!await IsSubscribed(userId, groupId))
            {
                var subscription = new GroupSubscription
                {
                    UserId = userId,
                    GroupId = groupId
                };
                await _repository.AddAsync(subscription);
                await _unitOfWork.Save(CancellationToken.None);
            }
        }

        public async Task Unsubscribe(int userId, int groupId)
        {
            var subscription = await _repository.Entities
                .FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
            
            if (subscription != null)
            {
                await _repository.DeleteAsync(subscription);
                await _unitOfWork.Save(CancellationToken.None);
            }
        }
    }
} 