document.addEventListener('DOMContentLoaded', function () {
    startConnection()
});

const match = document.cookie.match(new RegExp('(^| )AccessToken=([^;]+)'));
const token = match ? match[2] : null;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7107/CommonHub", {
        accessTokenFactory: () => token
    })
    .withAutomaticReconnect()
    .build();


connection.onclose(function () {
    if (window.location.pathname.toLowerCase().includes("/")
        && connection.state === signalR.HubConnectionState.Disconnected) {

        var h3 = document.getElementById('connection');
        h3.innerText = 'Disconnected';
    }

    console.log('ReConnecting in 5 Second ...');
    setTimeout(startConnection, 5000);
});

function startConnection() {
    if (connection.state === signalR.HubConnectionState.Disconnected) {
        connection.start().then(onConnected).catch(function (err) {
            console.log(err + `asdmasdmamdsmdmadmkalsmmdklmasdddddddddddddddddddddddddddddddadaldlmamdad`);
        });
    }
}

function onConnected() {

    if (window.location.pathname.toLowerCase().includes("/") &&
        connection.state === signalR.HubConnectionState.Connected) {

        var h3 = document.getElementById('connection');
        h3.innerText = 'Connected';
    }
}

/////////////////////////////////////////////////activities

// add sub activity
connection.on("PostSubActivity", function (subActivity) {
    if (window.location.pathname.toLowerCase().includes("/activities")) {
        const tablebody = document.queryselector("table tbody");

        const row = document.createelement("tr");
        row.id = `activity-row-${subActivity.id}`;

        row.innerhtml = `
        <td>${subActivity.title}</td>
        <td>${subActivity.description}</td>
        <td>${formatdate(subActivity.startdate)}</td>
        <td>${subActivity.duration ?? "n/a"}</td>
        <td>${subActivity.category}</td>
        <td id="status-${subActivity.id}">${subActivity.iscompleted ? "completed" : "pending"}</td>
        <td>
            ${false? `
                <a href="/activities/members?id=${subActivity.id}&isowner=true" class="btn btn-sm btn-info">members</a>
                <a href="/activities/comments?id=${subActivity.id}&isowner=true" class="btn btn-sm btn-info">comments</a>
                ${subActivity.parentid == null ? `<a href="/activities/addsub/${subActivity.id}" class="btn btn-sm btn-info">add sub activity</a>` : ""}
                <a href="/activities/edit/${subActivity.id}" class="btn btn-warning btn-sm">edit</a>
                <a href="/activities/remove/${subActivity.id}" class="btn btn-danger btn-sm">delete</a>
                ${!subActivity.iscompleted ? `<a href="/activities/complete/${subActivity.id}" class="btn btn-primary btn-sm">complete</a>` : ""}
            ` : `
                <a href="/activities/members?id=${subActivity.id}&isowner=false" class="btn btn-sm btn-info">members</a>
                <a href="/activities/comments?id=${subActivity.id}&isowner=false" class="btn btn-sm btn-info">comments</a>
                <a href="/activities/details/${subActivity.id}" class="btn btn-info btn-sm">details</a>
                <a href="/activities/leave/${subActivity.id}" class="btn btn-warning btn-sm">leave</a>
            `}
        </td>
    `;

        tablebody.appendchild(row);
    }
    else {
        alert("new activity")
    }
   
});

// Complete activity
connection.on("CompleteActivity", function (id) {

    if (window.location.pathname.toLowerCase().includes("/activities")) {

        const statusCell = document.getElementById(`status-${id}`);
        if (statusCell) {
            statusCell.textContent = "Completed";
        }

        const row = document.getElementById(`activity-row-${id}`);
        if (row) {
            const completeButton = row.querySelector(`a[href="/Activities/Complete/${id}"]`);
            if (completeButton) {
                completeButton.remove();
            }
        }
    }
    else {
        alert(`The activity with this ${id} has been completed.`);
    }
});

//remove
connection.on("RemoveActivity", function (id) {

    if (window.location.pathname.toLowerCase().includes("/activities")) {
        const row = document.getElementById(`activity-row-${id}`);
        if (row) {
            row.remove();
        }
    }
    else {
        alert(`The activity with this ${id} has been Deteled.`);
    }
});

//Exit member
connection.on("ExitMemberActivity", function (id, mId) {

    if (window.location.pathname.toLowerCase().includes("/activities/members")) {
        const row = document.getElementById(`member-row-${mId}`);
        if (row) {
            row.remove();
        }
    }
    else {
        alert(`A user with this ${mId} has left the activity with this ${id}`);
    }
});

//remove members
connection.on("RemoveMemberActivity", function (id, mId) {

    if (window.location.pathname.toLowerCase().includes("/activities/members")) {
        const row = document.getElementById(`member-row-${mId}`);
        if (row) {
            row.remove();
        }
    }
    else {
        alert(`A user with this ${mId} has removed the activity with this ${id}`);
    }
});

/////////////////////////////////////////////////projects

connection.on("AddActivityToProject", function (activity) {
    if (window.location.pathname.toLowerCase().includes("/projects/activities")) {
        const tableBody = document.querySelector("table tbody");

        const row = document.createElement("tr");
        row.id = `activity-row-${activity.id}`;

        row.innerHTML = `
        <td>${activity.title}</td>
        <td>${activity.description}</td>
        <td>${formatDate(activity.startDate)}</td>
        <td>${activity.duration ?? "N/A"}</td>
        <td>${activity.category}</td>
        <td id="status-${activity.id}">${activity.isCompleted ? "Completed" : "Pending"}</td>
        <td>
            ${false ? `
                <a href="/Activities/Members?id=${activity.id}&isOwner=true" class="btn btn-sm btn-info">Members</a>
                <a href="/Activities/Comments?id=${activity.id}&isOwner=true" class="btn btn-sm btn-info">Comments</a>
                ${activity.parentId == null ? `<a href="/Activities/AddSub/${activity.id}" class="btn btn-sm btn-info">Add sub activity</a>` : ""}
                <a href="/Activities/Edit/${activity.id}" class="btn btn-warning btn-sm">Edit</a>
                <a href="/Activities/Remove/${activity.id}" class="btn btn-danger btn-sm">Delete</a>
                ${!activity.isCompleted ? `<a href="/Activities/Complete/${activity.id}" class="btn btn-primary btn-sm">Complete</a>` : ""}
            ` : `
                <a href="/Activities/Members?id=${activity.id}&isOwner=false" class="btn btn-sm btn-info">Members</a>
                <a href="/Activities/Comments?id=${activity.id}&isOwner=false" class="btn btn-sm btn-info">Comments</a>
                <a href="/Activities/Details/${activity.id}" class="btn btn-info btn-sm">Details</a>
                <a href="/Activities/Leave/${activity.id}" class="btn btn-warning btn-sm">Leave</a>
            `}
        </td>
    `;

        tableBody.appendChild(row);
    }
    else {
        alert("You have been added to an activity.");
    }
});

connection.on("RemoveProject", function (id) {

    if (window.location.pathname.toLowerCase().includes("/projects")) {
        const row = document.getElementById(`project-row-${id}`);
        if (row) {
            row.remove();
        }
    }
    else {
        alert(`The activity with this ${id} has been Deteled.`);
    }
});

connection.on("ExitMemberProject", function (id, mId) {

    if (window.location.pathname.toLowerCase().includes("/projects/members")) {
        const row = document.getElementById(`member-row-${mId}`);
        if (row) {
            row.remove();
        }
    }
    else {
        alert(`A user with this ${mId} has left the project with this ${id}`);
    }
});

connection.on("RemoveMemberProject", function (id, mId) {

    if (window.location.pathname.toLowerCase().includes("/projects/members")) {
        const row = document.getElementById(`member-row-${mId}`);
        if (row) {
            row.remove();
        }
    }
    else {
        alert(`A user with this ${mId} has Removed the project with this ${id}`);
    }
});

/////////////////////////////////////////////////requests

//received requests
connection.on("RequestReceive", function (request) {
    if (window.location.pathname.toLowerCase().includes("/requests")) {

        const tableBody = document.querySelector("#requestsTable tbody");

        const row = document.createElement("tr");

        row.innerHTML = `
        <td>${request.senderId}</td>
        <td>${request.receiverId}</td>
        <td>${request.requestFor}</td>
        <td>${request.status === 0 ? `Pending` : request.status === 1 ? `Accepted` : `Rejected`}</td>
        <td>${formatDate(request.invitedAt)}</td>
        <td>${request.answeredAt ? formatDate(request.answeredAt) : "-"}</td>
        <td>${request.message ?? ""}</td>
        <td>
            ${request.status === 0 ? `
                <a href="/Requests/Answer?id=${request.id}&isAccepted=true" class="btn btn-warning">Accept</a>
                <a href="/Requests/Answer?id=${request.id}&isAccepted=false" class="btn btn-danger">Reject</a>
            ` : `<span class="text-muted">${request.status}</span>`}
        </td>
    `;

        tableBody.appendChild(row);
    }
    else {
        alert("A new  request received !! ")
    }
});

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString("sv-SE", { hour12: false }).replace(" ", " "); // yyyy-mm-dd HH:mm
}

/////////////////////////////////////////////////comments

connection.on("PostComment", function (request) {
    alert("a comment have been added")
});


