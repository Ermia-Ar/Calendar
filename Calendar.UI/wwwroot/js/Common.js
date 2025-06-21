document.addEventListener('DOMContentLoaded', function () {
    startConnection()
});

const match = document.cookie.match(new RegExp('(^| )AccessToken=([^;]+)'));
const token = match ? match[2] : null;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7107/CommonHub", {
        accessTokenFactory: () => token
    })
    .build();


connection.onclose(function () {
    console.log('ReConnecting in 5 Second ...');
    setTimeout(startConnection, 5000);
});

function startConnection() {
    if (connection.state === signalR.HubConnectionState.Disconnected) {
        connection.start().then(onConnected).catch(function (err) {
            console.log(err);
        });
    }
}

function onConnected() {
    var h3 = document.getElementById('connection');
    h3.innerText = 'Connected';
}

/////////////////////////////////////////////////activities

//received requests
connection.on("RequestReceive", function () {
    alert("a request received.");
});

// add sub activity
connection.on("PostSubActivity", function (activity) {

    alert("You have been added to an activity.");
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

    alert("You have been added to an activity.");
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


/////////////////////////////////////////////////comments

Connection.on("PostComment", function (request) {
    alert("a comment have been added")
});


