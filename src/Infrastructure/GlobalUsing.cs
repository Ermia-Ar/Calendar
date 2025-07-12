global using Core.Application.ApplicationServices.Activities.Queries.GetById;
global using Core.Application.ApplicationServices.Comments.Queries.GetAll;
global using Core.Application.ApplicationServices.Comments.Queries.GetById;
global using Core.Application.ApplicationServices.Projects.Queries.GetById;
global using Core.Application.ApplicationServices.Requests.Queries.GetAll;
global using Core.Application.ApplicationServices.Requests.Queries.GetById;
global using Core.Application.Common;

global using Core.Domain.Entities.Activities;
global using Core.Domain.Entities.Comments;
global using Core.Domain.Entities.Notifications;
global using Core.Domain.Entities.Projects;
global using Core.Domain.Entities.Requests;
global using Core.Domain.Enum;
global using Core.Domain.Filtering;
global using Core.Domain.Odering;
global using Core.Domain.Repositories;

global using Dapper;
global using Infrastructure.ExternalServices.CurrentUserServices;
global using Infrastructure.Persistence.Context;

global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.Data.SqlClient;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using SharedKernel.Dtos;
global using SharedKernel.Helper;
global using SharedKernel.Loggers;
global using SharedKernel.Loggers.Abstraction;
global using SharedKernel.QueryFilterings;
global using System.Collections.Immutable;
global using System.Data;
global using System.Security.Claims;
