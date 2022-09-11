﻿using Data;

namespace BookingApi.Features.Customer.Queries;

public class GetRoomById
{
    private readonly IUnitOfWork unitOfWork;

    public GetRoomById(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Model.Room? Handle(int id)
    {
        return unitOfWork.Rooms.Get(id);
    }
}