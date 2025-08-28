async function saveExcelFromByteArray(reportName, byte) {
    var blob = new Blob([byte], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    var link = document.createElement('a');
    var url = window.URL.createObjectURL(blob);
    link.href = url;
    link.download = reportName;
    link.click();
    window.URL.revokeObjectURL(url); // cleanup
}