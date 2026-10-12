function toggleCertificate() {
    const instructor = document.getElementById("roleInstructor");
    const certificate = document.getElementById("cert");
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
    document.getElementById("roleMember").addEventListener("change", toggleCertificate);

    document.getElementById("roleInstructor").addEventListener("change", toggleCertificate);

    toggleCertificate();
})