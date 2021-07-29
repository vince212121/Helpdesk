/*
\file:      call
\author:    Vincent Li
\purpose:   This is the js file for the call html page
*/

$(function () {
    // validate call modal
    document.addEventListener("keyup", e => {
        $("#modalstatus").removeClass(); // remove any existing css on div
        if ($("#CallModalForm").valid()) {
            $("#modalstatus").attr("class", "badge badge-success"); // green badge will appear for valid data
            $("#modalstatus").text("data entered is valid");
            $("#actionbutton").prop('disabled', false); // enable action button
        }
        else {
            $("#modalstatus").attr("class", "badge badge-danger"); // red badge will appear for invalid
            $("#modalstatus").text("fix errors");
            $("#actionbutton").prop('disabled', true); // disable action button
        }
    });
    // making sure the conditions are met
    $("#CallModalForm").validate({
        rules: {
            ddlPros: { required: true },
            ddlEmps: { required: true },
            ddlTechs: { required: true },
            NoteArea: { maxlength: 250, minlength: 1, required: true }
        },
        errorElement: "div",
        messages: {
            ddlPros: {
                required: "select Problem"
            },
            ddlEmps: {
                required: "select Employee"
            },
            ddlTechs: {
                required: "select Tech"
            },
            NoteArea: {
                required: "required 1-250 chars.", minlength: "required 1-250 chars.", maxlength: "required 1-250 chars."
            }
        }
    });

    // get all
    const getAll = async (msg) => {
        try {
            $(`#callList`).text(`Finding call Information...`);
            let response = await fetch(`api/call`);
            if (response.ok) {
                let data = await response.json(); // this returns a promise so we await it
                buildcallList(data);// builds calls
                //loadDepartmentDDL(); // loads departments

                msg === `` ? // are we appending to an existing message
                    $("#status").text("calls loaded") : $("#status").text(`${msg}`);
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // 404 not found 
                $("#status").text("no such path on server");
            }// else

            // get employee info
            response = await fetch(`api/employee`);
            if (response.ok) {
                let divs = await response.json(); // this returns a promise so we await it
                sessionStorage.setItem("allemployees", JSON.stringify(divs));
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // 404 not found 
                $("#status").text("no such path on server");
            }// else

            // get problem info
            response = await fetch(`api/problem`);
            if (response.ok) {
                let divs = await response.json(); // this returns a promise so we await it
                sessionStorage.setItem("allproblems", JSON.stringify(divs));
            } else if (response.status !== 404) { // probably some other client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // 404 not found 
                $("#status").text("no such path on server");
            }// else

        } catch (error) {
            $("#status").text(error.message);
        }
    }; // getall

    // employee ddl stuff
    const loadEmployeeDDL = (emp) => {
        html = '';
        html += `<option value="" disabled selected>Select an Employee</option>`; // default option
        $('#ddlEmps').empty();
        let allemployees = JSON.parse(sessionStorage.getItem('allemployees'));
        allemployees.map(div => html += `<option value="${div.id}">${div.lastname}</option>`);
        $('#ddlEmps').append(html);
        $('#ddlEmps').val(emp);
    }
    // tech ddl stuff
    const loadTechDDL = (emp) => {
        html = '';
        html += `<option value="" disabled selected>Select a Technician</option>`; // default option
        $('#ddlTechs').empty();
        let alltechnicians = JSON.parse(sessionStorage.getItem('allemployees'));
        alltechnicians.map(tech =>
            html += tech.isTech ? `<option value="${tech.id}">${tech.lastname}</option>` : ``); // checking which one is a valid technician
        $('#ddlTechs').append(html);
        $('#ddlTechs').val(emp);
    }
    // problem ddl stuff
    const loadProblemDDL = (emp) => {
        html = '';
        html += `<option value="" disabled selected>Select a Problem</option>`; // default option
        $('#ddlPros').empty();
        let allproblems = JSON.parse(sessionStorage.getItem('allproblems'));
        allproblems.map(div => html += `<option value="${div.id}">${div.desc}</option>`);
        $('#ddlPros').append(html);
        $('#ddlPros').val(emp);
    }

    // setting up the update
    const setupForUpdate = (id, data) => {
        $("#actionbutton").val("update");
        $("#modaltitle").html("<h4>update call</h4>");

        clearModalFields();

        data.map(call => {
            if (call.id === parseInt(id)) {
                $("#theModal").modal("toggle");
                $("#deletebutton").show();
                $("#checkBoxClose").show();
                $("#closeText").show();
                $("#labelDateClosed").show();
                $("#closeDatebox").show();

                $('#NoteArea').val(call.notes);

                // loading ddl
                loadEmployeeDDL(call.employeeId.toString());
                loadProblemDDL(call.problemId.toString());
                loadTechDDL(call.techId.toString());

                // checking time
                sessionStorage.setItem("dateOpened", formatDate(call.dateOpened));
                $('#labelDateOpened').text(formatDate(call.dateOpened).replace("T"," "));
                //$('#dateOpened').val(call.dateOpened);

                // for new closes
                sessionStorage.setItem("newClose", formatDate());

                // adding closed date if there is one
                if (call.dateClosed != null) {
                    $('#labelDateClosed').text(formatDate(call.dateClosed).replace("T", " "));
                    sessionStorage.setItem("dateClosed", formatDate(call.dateClosed));
                }
               

                // check if the status is not false, if it is disable the stuff
                if (!call.openStatus) {
                    $('#labelDateClosed').text(formatDate(call.dateClosed));
                    //$('#dateClosed').val(call.dateClosed);
                    $('#checkBoxClose').prop('checked', true);
                    $('#checkBoxClose').prop('disabled', true);
                    $('#ddlPros').prop('disabled', true);
                    $('#ddlEmps').prop('disabled', true);
                    $('#ddlTechs').prop('disabled', true);
                    $('#NoteArea').attr('readonly', 'readonly');
                    $('#actionbutton').hide();
                }

                // storing session data
                sessionStorage.setItem('id', call.id);
                sessionStorage.setItem('timer', call.timer);

                $("#modalstatus").text("update data");

                // enable the button again if it is doing an update
                $("#actionbutton").prop('disabled', false); // enable action button
            } // if
        }); //datamap
    }; //setup for update

    // clears fields
    const clearModalFields = () => {
        $('#ddlPros').val('');
        $('#ddlEmps').val('');
        $('#ddlTechs').val('');
        $('#labelDateOpened').text('');
        $('#dateOpened').val('');
        $('#labelDateClosed').text('');
        $('#dateClosed').val('');

        $('#checkBoxClose').prop('checked', false);
        $('#NoteArea').val('');

        // need to delete session storage stuff as well
        sessionStorage.removeItem('id');
        sessionStorage.removeItem('timer');
        sessionStorage.removeItem("dateClosed");
        sessionStorage.removeItem("dateOpened");

        // making it editable
        $('#ddlPros').prop('disabled', false);
        $('#ddlEmps').prop('disabled', false);
        $('#ddlTechs').prop('disabled', false);
        $('#NoteArea').attr('readonly', false);
        $('#checkBoxClose').prop('disabled', false);

        // showing button
        $('#actionbutton').show();
        $('#deletebutton').show();

    }; // clear modal fields

    // the formated date thing
    const formatDate = (date) => {
        let d;
        (date === undefined) ? d = new Date() : d = new Date(Date.parse(date));

        let _day = d.getDate();
        if (_day < 10) { _day = "0" + _day; }

        let _month = d.getMonth() + 1;
        if (_month < 10) { _month = "0" + _month; }

        let _year = d.getFullYear();

        let _hour = d.getHours();
        if (_hour < 10) { _hour = "0" + _hour; }

        let _min = d.getMinutes();
        if (_min < 10) { _min = "0" + _min; }

        let _second = d.getSeconds();
        if (_second < 10) { _second = "0" + _second; }

        return _year + "-" + _month + "-" + _day + "T" + _hour + ":" + _min + ":" + _second; 
    };

    // setting up the add
    const setupForAdd = () => {
        $("#actionbutton").val("add");
        $("#modaltitle").html("<h4>add call info</h4>");
        $("#theModal").modal("toggle");
        $("#modalstatus").text("add new call");

        clearModalFields();
        $("#deletebutton").hide();
        $("#checkBoxClose").hide();
        $("#closeText").hide();
        $("#labelDateClosed").hide();
        $("#closeDatebox").hide();
        

        // reset ddl 
        loadProblemDDL(-1);
        loadEmployeeDDL(-1);
        loadTechDDL(-1);

        // setting the date/time thing
        $('#labelDateOpened').text(formatDate().replace("T", " "));
        $('#dateOpened').val(formatDate().replace("T", " "));
        sessionStorage.setItem("dateOpened", formatDate());

    }; //setupforadd


    // add method
    const add = async () => {
        try {
            call = new Object();
            call.problemId = parseInt($('#ddlPros').val());
            call.employeeId = parseInt($('#ddlEmps').val());
            call.techId = parseInt($('#ddlTechs').val());

            // set time
            call.dateOpened = sessionStorage.getItem("dateOpened");

            // checking if it can be used or not
            if ($('#checkBoxClose').is(':checked')) {
                call.openStatus = false;
                // using the dateOpened as the close date if they select it
                call.dateClosed = sessionStorage.getItem("dateOpened");
            }
            else { // means it is not closed
                call.openStatus = true;
            }

            call.Notes = $('#NoteArea').val();

            call.Id = -1; // hard code to overwrite when data is inserted into the database

            // send the call info to the server asynchronously using POST
            let response = await fetch("api/call", {
                method: "POST",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(call)
            });

            if (response.ok) // or check for response status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            }// else

        } catch (error) {
            $("#status").text(error.message);
            console.table(error);
        } // try/catch
        $("#theModal").modal("toggle");
    }; // add

    // update function
    const update = async () => {
        try {
            call = new Object();
            call.problemId = parseInt($('#ddlPros').val());
            call.employeeId = parseInt($('#ddlEmps').val());
            call.techId = parseInt($('#ddlTechs').val());
            call.dateOpened = sessionStorage.getItem("dateOpened");
            

            // checking status
            if ($('#checkBoxClose').is(':checked')) {
                call.openStatus = false;
                call.dateClosed = sessionStorage.getItem("newClose");
            } else {
                call.openStatus = true;
                call.dateClosed = sessionStorage.getItem("dateClosed");
            }
            call.notes = $('#NoteArea').val();
            call.id = parseInt(sessionStorage.getItem('id'));
            call.timer = sessionStorage.getItem('timer');

            // send the updated back to the server asynchronously using PUT
            let response = await fetch("api/call", {
                method: "PUT",
                headers: { "Content-Type": "application/json; charset=utf-8" },
                body: JSON.stringify(call)
            });

            if (response.ok) // or check for response status
            {
                let data = await response.json();
                getAll(data.msg);
            } else if (response.status !== 404) { // probably some client side error
                let problemJson = await response.json();
                errorRtn(problemJson, response.status);
            } else { // else 404 not found
                $("#status").text("no such path on server");
            }// else

        } catch (error) {
            $("#status").text(error.message);
            console.table(error);
        } // try/catch
        $("#theModal").modal("toggle");

    }; // update

    //aciton button for either add or update
    $("#actionbutton").click(() => {
        if ($('#CallModalForm').valid()) {
            $("#actionbutton").val() === "update" ? update() : add();
        }
    });


    // builds callList, modified to work with search
    const buildcallList = (data, usealldata = true) => {
        $("#callList").empty();
        div = $(`<div class="list-group-item text-white bg-secondary row d-flex" id="status">employee Info</div>
                  <div class="list-group-item row d-flex text-center" id="heading">
                  <div class="col-4 h4">Date</div>
                  <div class="col-4 h4">For</div>
                  <div class="col-4 h4">Problem</div>
                </div>`);
        div.appendTo($("#callList"));
        usealldata ? sessionStorage.setItem("allcalls", JSON.stringify(data)) : null; // storing employee data to session storage
        btn = $(`<button class="list-group-item row d-flex" id="0"><div class="col-12 text-left">...click to add call</div></button>`);
        btn.appendTo($("#callList"));
        data.map(call => {
            btn = $(`<button class="list-group-item row d-flex" id="${call.id}">`);
            btn.html(`<div class="col-4" id ="openDate${call.id}">${call.dateOpened}</div>
                      <div class="col-4" id ="employeefname${call.id}">${call.employeeName}</div> 
                      <div class="col-4" id ="employeelname${call.id}">${call.problemDescription}</div>`
            );
            btn.appendTo($("#callList"));
        }); // map
    } // build employee list

    // when clicking on list
    $(`#callList`).click((e) => {

        // Keep track of where they clicked
        if (!e) e = window.event;
        let id = e.target.parentNode.id;
        if (id === `callList` || id === ``) {
            id = e.target.id;
        } // clicked on row somewhere else

        // making sure they clicked on the list and not the heading or status
        if (id !== "status" && id !== "heading") {
            let data = JSON.parse(sessionStorage.getItem("allcalls")); // currently have the employee id, now it is going to look at all the employees from the session storage
            id === "0" ? setupForAdd() : setupForUpdate(id, data);
        } else {
            return false; // ignore if they click on heading or status
        }
    }); // callList click

    // jquery finds data then gets confirmation
    // look at bootstrap js library for delete confirmation
    // Basically, it needs to be the same to delete properly
    $('[data-toggle=confirmation]').confirmation({ rootSelector: '[data-toggle=confirmation]' });
    // more confirmation stuff
    $('#deletebutton').click(() => _delete()); // if yes was chosen

    // delete function
    const _delete = async () => {
        try {
            // deleting call instead of employee
            let response = await fetch(`api/call/${sessionStorage.getItem('id')}`, { 
                method: 'DELETE',
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            });
            if (response.ok) // or check for reponse.status
            {
                let data = await response.json();
                getAll(data.msg);
            } else {
                $('#status').text(`Status - ${response.status}, Problem on delete server side, see server side console`);
            }
            $('#theModal').modal('toggle');
        } catch (error) {
            $('#status').text(error.message);
        }
    }; // delete

    // Search
    $("#srch").keyup(() => {
        let alldata = JSON.parse(sessionStorage.getItem("allcalls"));
        let filtereddata = alldata.filter((call) => call.employeeName.match(new RegExp($("#srch").val(), 'i'))); // filtering by employeeName
        buildcallList(filtereddata, false);
    }); // srch keyup

    getAll(""); // first grab the data from the server
}); // jQuery ready method

// server was reached out but server had a problem with the call
const errorRtn = (problemJson, status) => {
    if (status > 499) {
        $("#status").text("Problem server side, see debug console");
    } else {
        let keys = Object.keys(problemJson.errors)
        problem = {
            status: status,
            statusText: problemJson.errors[keys[0]][0], // first error
        };
        $("#status").text("Problem client side, see browser console");
        console.log(problem);
    } // else
}