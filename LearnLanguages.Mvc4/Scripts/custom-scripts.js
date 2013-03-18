
//Attaches accordion to all classes marked accordion
$(document).ready(function DoSomething() {

  $("a").click(function ShowAlertDummy() {
    alert("jquery script was run!");
  });

  $("#accordion").accordion();
});