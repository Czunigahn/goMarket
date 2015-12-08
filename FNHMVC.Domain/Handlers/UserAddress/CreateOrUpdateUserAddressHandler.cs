using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNHMVC.CommandProcessor.Command;
using FNHMVC.Data.Infrastructure;
using FNHMVC.Data.Repositories;
using FNHMVC.Domain.Commands;

namespace FNHMVC.Domain.Handlers
{

    public class CreateOrUpdateUserAddressHandler : ICommandHandler<CreateOrUpdateUserAddressCommand>
    {
        private readonly IUserAddressRepository userAddressRepository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOrUpdateUserAddressHandler(IUserAddressRepository userAddressRepository, IUnitOfWork unitOfWork)
        {
            this.userAddressRepository = userAddressRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateUserAddressCommand command)
        {
            var userAddress = new FNHMVC.Model.UserAddress
            {
                AddressLine1 = command.AddressLine1,
                AddressLine2 = command.AddressLine2,
                City = command.City,
                Country = command.Country,
                FullName = command.FullName,
                User = command.User,
                PhoneNumber = command.PhoneNumber,
                State = command.State,
                UserAddressId = command.UserAddressId,
                ZipCode = command.ZipCode,
                Activated = command.Activated
            };

            if (userAddress.UserAddressId == 0)
                userAddressRepository.Add(userAddress);
            else
                userAddressRepository.Update(userAddress);

            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
