﻿using Core.Domain.Enums;

namespace Core.Domain.Entities;

public sealed class RoomStatus
{
    public required RoomStatusId Id { get; init; }
    public required string Name { get; init; }
    public required string Label { get; init; }
}