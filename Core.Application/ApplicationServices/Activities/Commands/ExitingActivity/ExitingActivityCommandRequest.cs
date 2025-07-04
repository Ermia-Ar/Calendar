﻿using MediatR;

namespace Core.Application.ApplicationServices.Activities.Commands.ExitingActivity;

public record class ExitingActivityCommandRequest(
    string ActivityId
    ) : IRequest;
