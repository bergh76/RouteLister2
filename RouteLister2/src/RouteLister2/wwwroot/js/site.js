// Write your Javascript code.
// < datatables VEHICLE 1 API start
$(document).ready(function () {
    $('#carOne').DataTable({
        dom: 'B<"clear"><lf<t>ip>',
        buttons: [{ extend: 'copy', text: 'Kopiera', },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: { modifier: { page: 'current' } },
            },
        {
            //extend: 'pdf',
            extend: 'pdfHtml5',
            text: 'Pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL'
        },
        ],
        "language": {
            "lengthMenu": "Visa _MENU_ rader per sida",
            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
            "info": "_PAGE_ av _PAGES_",
            "infoEmpty": "Datatabellen är tom",
            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
        },
        "processing": true,
        "order": [[0, "asc"], [4, 'asc']]
    })

});
// datatables API end

// < datatables VEHICLE 2 API start
$(document).ready(function () {
    $('#carTwo').DataTable({
        dom: 'B<"clear"><lf<t>ip>',
        buttons: [{ extend: 'copy', text: 'Kopiera', },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: { modifier: { page: 'current' } },
            },
        {
            //extend: 'pdf',
            extend: 'pdfHtml5',
            text: 'Pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL'
        },
        ],
        "language": {
            "lengthMenu": "Visa _MENU_ rader per sida",
            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
            "info": "_PAGE_ av _PAGES_",
            "infoEmpty": "Datatabellen är tom",
            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
        },
        "processing": true,
        "order": [[0, "asc"], [4, 'asc']]
    })

});
// datatables API end
// < datatables VEHICLE 3 API start
$(document).ready(function () {
    $('#carThree').DataTable({
        dom: 'B<"clear"><lf<t>ip>',
        buttons: [{ extend: 'copy', text: 'Kopiera', },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: { modifier: { page: 'current' } },
            },
        {
            //extend: 'pdf',
            extend: 'pdfHtml5',
            text: 'Pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL'
        },
        ],
        "language": {
            "lengthMenu": "Visa _MENU_ rader per sida",
            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
            "info": "_PAGE_ av _PAGES_",
            "infoEmpty": "Datatabellen är tom",
            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
        },
        "processing": true,
        "order": [[0, "asc"], [4, 'asc']]
    })

});
// datatables API end
// < datatables VEHICLE 4 API start
$(document).ready(function () {
    $('#carFour').DataTable({
        dom: 'B<"clear"><lf<t>ip>',
        buttons: [{ extend: 'copy', text: 'Kopiera', },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: { modifier: { page: 'current' } },
            },
        {
            //extend: 'pdf',
            extend: 'pdfHtml5',
            text: 'Pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL'
        },
        ],
        "language": {
            "lengthMenu": "Visa _MENU_ rader per sida",
            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
            "info": "_PAGE_ av _PAGES_",
            "infoEmpty": "Datatabellen är tom",
            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
        },
        "processing": true,
        "order": [[0, "asc"], [4, 'asc']]
    })

});
// datatables API end

// < datatables VEHICLE 5 API start
$(document).ready(function () {
    $('#carFive').DataTable({
        dom: 'B<"clear"><lf<t>ip>',
        buttons: [{ extend: 'copy', text: 'Kopiera', },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: { modifier: { page: 'current' } },
            },
        {
            //extend: 'pdf',
            extend: 'pdfHtml5',
            text: 'Pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL'
        },
        ],
        "language": {
            "lengthMenu": "Visa _MENU_ rader per sida",
            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
            "info": "_PAGE_ av _PAGES_",
            "infoEmpty": "Datatabellen är tom",
            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
        },
        "processing": true,
        "order": [[0, "asc"], [4, 'asc']]
    })

});
// datatables API end
// < datatables VEHICLE 6 API start
$(document).ready(function () {
    $('#carSix').DataTable({
        dom: 'B<"clear"><lf<t>ip>',
        buttons: [{ extend: 'copy', text: 'Kopiera', },
            {
                extend: 'excel',
                text: 'Excel',
                exportOptions: { modifier: { page: 'current' } },
            },
        {
            //extend: 'pdf',
            extend: 'pdfHtml5',
            text: 'Pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL'
        },
        ],
        "language": {
            "lengthMenu": "Visa _MENU_ rader per sida",
            "zeroRecords": "Din sökning gav inget - försök med ett annat sökord",
            "info": "_PAGE_ av _PAGES_",
            "infoEmpty": "Datatabellen är tom",
            "infoFiltered": "(filtrerdad på _MAX_ antal poster)"
        },
        "processing": true,
        "order": [[0, "asc"], [4, 'asc']]
    })

});
// datatables API end