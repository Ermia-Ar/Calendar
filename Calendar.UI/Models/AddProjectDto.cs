﻿namespace Calendar.UI.Models;

public class AddProjectDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Massage { get; set; }
    public string[]? MemberIds { get; set; }
}

