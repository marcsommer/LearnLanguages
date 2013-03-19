
//Attaches accordion to all classes marked accordion
$(document).ready(function () {

  $("#testlink").click(function ShowAlertDummy() {
    alert("jquery script is a runnin!");
  });

  AttachAccordion();
});

function AttachAccordion() {
  $("#accordion").accordion({ collapsible: true });
}

