﻿namespace Core.Domain.Enum
{
    public enum RequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public enum UserCategory
    {
        Employee , 
        Student ,
    }

    public enum ActivityCategory
    {
        Task,
        Event,
    }

    public enum RequestFor
    {
        Activity ,
        Project
    }

    public enum RecurrenceType
    {
        None, 
        Daily, 
        Weekly, 
        Monthly, 
        Custom 
    }
}
