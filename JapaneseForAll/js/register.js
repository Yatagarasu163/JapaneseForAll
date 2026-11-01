function toggleCertificate() {
    const instructor = document.getElementById(roleInstructorId);
    const certificate = document.getElementById(certificateId);
    const certSection = document.querySelector(".cert");

    if (!instructor || !certificate || !certSection) {
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
    const certificate = document.getElementById(certificateId);

    if (member && instructor) {
        member.addEventListener("change", toggleCertificate);
        instructor.addEventListener("change", toggleCertificate);

        toggleCertificate();
    }

    if (certificate) {
        certificate.addEventListener("change", function () {
            console.log("File selected:", certificate.files[0]?.name);
        });
    }
});