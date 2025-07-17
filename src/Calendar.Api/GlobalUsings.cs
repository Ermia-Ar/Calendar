global using Core.Application.ApplicationServices.Activities.Commands.Add;
global using Core.Application.ApplicationServices.Activities.Commands.AddSubActivity;
global using Core.Application.ApplicationServices.Activities.Commands.Complete;
global using Core.Application.ApplicationServices.Activities.Commands.Remove;
global using Core.Application.ApplicationServices.Activities.Commands.RemoveMember;
global using Core.Application.ApplicationServices.Activities.Commands.SubmitRequest;
global using Core.Application.ApplicationServices.Activities.Commands.UpdateActivity;
global using Core.Application.ApplicationServices.Activities.Queries.GetAll;
global using Core.Application.ApplicationServices.Activities.Queries.GetById;
global using Core.Application.ApplicationServices.Activities.Queries.GetMembers;
global using Core.Application.ApplicationServices.Auth.Commands.Login;
global using Core.Application.ApplicationServices.Auth.Queries.GetAll;
global using Core.Application.ApplicationServices.Comments.Commands.Add;
global using Core.Application.ApplicationServices.Comments.Commands.Remove;
global using Core.Application.ApplicationServices.Comments.Commands.Update;
global using Core.Application.ApplicationServices.Comments.Queries.GetAll;
global using Core.Application.ApplicationServices.Comments.Queries.GetById;
global using Core.Application.ApplicationServices.Projects.Commands.Add;
global using Core.Application.ApplicationServices.Projects.Commands.AddActivity;
global using Core.Application.ApplicationServices.Projects.Commands.Exiting;
global using Core.Application.ApplicationServices.Projects.Commands.Remove;
global using Core.Application.ApplicationServices.Projects.Commands.RemoveMember;
global using Core.Application.ApplicationServices.Projects.Commands.AddMembers;
global using Core.Application.ApplicationServices.Projects.Queries.GetAll;
global using Core.Application.ApplicationServices.Projects.Queries.GetById;
global using Core.Application.ApplicationServices.Projects.Queries.GetMembers;
global using Core.Application.ApplicationServices.Requests.Commands.Remove;
global using Core.Application.ApplicationServices.Requests.Queries.GetAll;
global using Core.Application.ApplicationServices.Requests.Queries.GetById;


global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.SignalR;

global using SharedKernel.QueryFilterings;
global using SharedKernel.ResponseResult;
