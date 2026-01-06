// Tour Management System JavaScript

document.addEventListener('DOMContentLoaded', function() {
    console.log('Tour Management System initialized');

    const deleteButtons = document.querySelectorAll('.btn-delete');
    deleteButtons.forEach(button => {
        button.addEventListener('click', function(e) {
            if (!confirm('Are you sure you want to delete this item?')) {
                e.preventDefault();
            }
        });
    });

    const fileInputs = document.querySelectorAll('input[type="file"]');
    fileInputs.forEach(input => {
        input.addEventListener('change', function(e) {
            const fileName = e.target.files[0]?.name;
            const label = e.target.nextElementSibling;
            if (label && fileName) {
                label.textContent = fileName;
            }
        });
    });
});
