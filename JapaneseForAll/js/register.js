function toggleCertificate() {
    if (!instructor || !certificate || !certSection || !member) {
        console.error("Certificate controls could not be found.");
        return;
    }

    const showCertificate = instructor.checked;

    certSection.style.display = showCertificate ? "block" : "none";
    certificate.disabled = !showCertificate;
}

document.addEventListener("DOMContentLoaded", function () {
    const member = document.getElementById(roleMemberId);
    const instructor = document.getElementById(roleInstructorId);

    if (member && instructor) {
        member.addEventListener("change", toggleCertificate);
        instructor.addEventListener("change", toggleCertficate);
        toggleCertificate();
    }
})
document.addEventListener("DOMContentLoaded", function () {
    const certificate = document.getElementById("cert");

    if (certificate) {
        certificate.addEventListener("change", function () {
            console.log("File selected:", certificate.files[0]?.name);
        });
    }
});
