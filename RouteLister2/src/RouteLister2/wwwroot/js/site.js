// Write your Javascript code.
/* Formatting function for row details - modify as you need */
//function format(d) {
//    // `d` is the original data object for the row
//    return '<table cellpadding="5" cellspacing="0" border="0" style="padding-left:50px;">' +
//       '<tr>' +
//            '<td>Datum:</td>1' +
//            '<td><input class="form-control" type="text" id="date" value="' + d.Datum + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>Antal:</td>' +
//            '<td><input class="form-control" type="text" id="amount" value="' + d.Antal + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//             '<td>Namn:</td>' +
//            '<td><input class="form-control" type="text" id="name" value="' + d.Namn + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>Adress:</td>' +
//            '<td><input class="form-control" type="text" id="adress" value="' + d.Adress + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>Stad:</td>' +
//            '<td><input class="form-control" type="text" id="sity" value="' + d.Stad + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>Postnr:</td>' +
//            '<td><input class="form-control" type="text" id="areacode" value="' + d.Postnr + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>Leverantor:</td>' +
//            '<td><input class="form-control" type="text" id="supplyer" value="' + d.Leverantor + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>KollieId:</td>' +
//            '<td><input class="form-control" type="text" id="collieid" value="' + d.KollieId + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>Artikel:</td>' +
//            '<td><input class="form-control" type="text" id="article" value="' + d.Artikel + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>Tilldelad:</td>' +
//            '<td><input class="form-control" type="text" id="assigned" value="' + d.Tilldelad + '"  /></td>' +
//       '</tr>' +
//       '<tr>' +
//            '<td>Spara:</td><td><button class="btn btn-xs btn-success fa fa-save pull-right"></button></td>' +
//       '</tr>' +
//       '</table>';
//}
//// < datatables VEHICLE 1 API start
//$(document).ready(function () {
//    //$('#carOne').DataTable({
//    var table = $('#carOne').DataTable({
//        "ajax": "../testdata/testdatatables.json",
//        "columns": [
//            {
//                "className": 'details-control',
//                "orderable": false,
//                "data": null,
//                "defaultContent": ''
//            },
//            { "data": "Datum" },
//            { "data": "Antal" },
//            { "data": "Namn" },
//            { "data": "Adress" },
//            { "data": "Stad" },
//            { "data": "Postnr" },
//            { "data": "Leverantor" },
//            { "data": "KollieId" },
//            { "data": "Artikel" },
//            { "data": "Tilldelad" },
//            {
//                "className": 'details-control',
//                "orderable": false,
//                "data": null,
//                "defaultContent": ''
//            }
//        ],
//        dom: 'B<"clear"><lf<t>ip>',
//        buttons: [{ extend: 'copy', text: 'Kopiera', },
//            {
//                extend: 'excel',
//                text: 'Excel',
//                exportOptions: { modifier: { page: 'current' } },
//            },
//            {
//                //extend: 'pdf',
//                extend: 'pdfHtml5',
//                text: 'Pdf',
//                orientation: 'landscape',
//                pageSize: 'LEGAL'
//            },
//        ],
//        "language": {
//            "lengthMenu": "Visa _MENU_ rader per sida",
//            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
//            "info": "_PAGE_ av _PAGES_",
//            "infoEmpty": "Datatabellen är tom",
//            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
//        },
//        "processing": true,
//        "order": [[1, "asc"], [6, 'asc']]
//    })
//    console.log(table);
//    $('#carOne tbody').on('click', 'td.details-control', function () {
//        var tr = $(this).closest('tr');
//        var row = table.row(tr);

//        if (row.child.isShown()) {
//            // This row is already open - close it
//            row.child.hide();
//            tr.removeClass('shown');
//        }
//        else {
//            // Open this row
//            row.child(format(row.data())).show();
//            tr.addClass('shown');
//        }
//    });
//});
//// datatables API end

//// < datatables VEHICLE 2 API start
//$(document).ready(function () {
//    $('#carTwo').DataTable({
//        dom: 'B<"clear"><lf<t>ip>',
//        buttons: [{ extend: 'copy', text: 'Kopiera', },
//            {
//                extend: 'excel',
//                text: 'Excel',
//                exportOptions: { modifier: { page: 'current' } },
//            },
//        {
//            //extend: 'pdf',
//            extend: 'pdfHtml5',
//            text: 'Pdf',
//            orientation: 'landscape',
//            pageSize: 'LEGAL'
//        },
//        ],
//        "language": {
//            "lengthMenu": "Visa _MENU_ rader per sida",
//            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
//            "info": "_PAGE_ av _PAGES_",
//            "infoEmpty": "Datatabellen är tom",
//            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
//        },
//        "processing": true,
//        "order": [[0, "asc"], [4, 'asc']]
//    })

//});
//// datatables API end
//// < datatables VEHICLE 3 API start
//$(document).ready(function () {
//    $('#carThree').DataTable({
//        dom: 'B<"clear"><lf<t>ip>',
//        buttons: [{ extend: 'copy', text: 'Kopiera', },
//            {
//                extend: 'excel',
//                text: 'Excel',
//                exportOptions: { modifier: { page: 'current' } },
//            },
//        {
//            //extend: 'pdf',
//            extend: 'pdfHtml5',
//            text: 'Pdf',
//            orientation: 'landscape',
//            pageSize: 'LEGAL'
//        },
//        ],
//        "language": {
//            "lengthMenu": "Visa _MENU_ rader per sida",
//            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
//            "info": "_PAGE_ av _PAGES_",
//            "infoEmpty": "Datatabellen är tom",
//            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
//        },
//        "processing": true,
//        "order": [[0, "asc"], [4, 'asc']]
//    })

//});
//// datatables API end
//// < datatables VEHICLE 4 API start
//$(document).ready(function () {
//    $('#carFour').DataTable({
//        dom: 'B<"clear"><lf<t>ip>',
//        buttons: [{ extend: 'copy', text: 'Kopiera', },
//            {
//                extend: 'excel',
//                text: 'Excel',
//                exportOptions: { modifier: { page: 'current' } },
//            },
//        {
//            //extend: 'pdf',
//            extend: 'pdfHtml5',
//            text: 'Pdf',
//            orientation: 'landscape',
//            pageSize: 'LEGAL'
//        },
//        ],
//        "language": {
//            "lengthMenu": "Visa _MENU_ rader per sida",
//            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
//            "info": "_PAGE_ av _PAGES_",
//            "infoEmpty": "Datatabellen är tom",
//            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
//        },
//        "processing": true,
//        "order": [[0, "asc"], [4, 'asc']]
//    })

//});
//// datatables API end

//// < datatables VEHICLE 5 API start
//$(document).ready(function () {
//    $('#carFive').DataTable({
//        dom: 'B<"clear"><lf<t>ip>',
//        buttons: [{ extend: 'copy', text: 'Kopiera', },
//            {
//                extend: 'excel',
//                text: 'Excel',
//                exportOptions: { modifier: { page: 'current' } },
//            },
//        {
//            //extend: 'pdf',
//            extend: 'pdfHtml5',
//            text: 'Pdf',
//            orientation: 'landscape',
//            pageSize: 'LEGAL'
//        },
//        ],
//        "language": {
//            "lengthMenu": "Visa _MENU_ rader per sida",
//            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
//            "info": "_PAGE_ av _PAGES_",
//            "infoEmpty": "Datatabellen är tom",
//            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
//        },
//        "processing": true,
//        "order": [[0, "asc"], [4, 'asc']]
//    })

//});
//// datatables API end
//// < datatables VEHICLE 6 API start
//$(document).ready(function () {
//    $('#carSix').DataTable({
//        dom: 'B<"clear"><lf<t>ip>',
//        buttons: [{ extend: 'copy', text: 'Kopiera', },
//            {
//                extend: 'excel',
//                text: 'Excel',
//                exportOptions: { modifier: { page: 'current' } },
//            },
//        {
//            //extend: 'pdf',
//            extend: 'pdfHtml5',
//            text: 'Pdf',
//            orientation: 'landscape',
//            pageSize: 'LEGAL'
//        },
//        ],
//        "language": {
//            "lengthMenu": "Visa _MENU_ rader per sida",
//            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
//            "info": "_PAGE_ av _PAGES_",
//            "infoEmpty": "Datatabellen är tom",
//            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
//        },
//        "processing": true,
//        "order": [[0, "asc"], [4, 'asc']]
//    })

//});
//// datatables API end

//// ROUTELIST START //
//document.getElementById("carOne").addEventListener("click", function (e) {
//    if (e.target.tagName === "A") {
//        e.preventDefault();
//        var row = e.target.parentNode.parentNode;
//        while ((row = nextTr(row)) && !/\bparent\b/.test(row.className))
//            toggle_it(row);
//    }
//});

//function nextTr(row) {
//    while ((row = row.nextSibling) && row.nodeType != 1);
//    return row;
//}

//function toggle_it(item) {
//    if (/\bopen\b/.test(item.className))
//        item.className = item.className.replace(/\bopen\b/, " ");
//    else
//        item.className += " open";
//}