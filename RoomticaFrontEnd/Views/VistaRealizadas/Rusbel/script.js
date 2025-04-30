document.addEventListener('DOMContentLoaded', function() {
    const buttons = document.querySelectorAll('.interactive-button');

    buttons.forEach(button => {
        button.addEventListener('mouseover', () => {
            button.classList.add('highlight');
        });

        button.addEventListener('mouseout', () => {
            button.classList.remove('highlight');
        });

        button.addEventListener('click', () => {
            handleButtonClick(button.dataset.action);
        });
    });

    document.getElementById('consult-button').addEventListener('click', consultDocument);
    document.getElementById('register-additional-guest').addEventListener('click', registerAdditionalGuest);
});

function handleButtonClick(action) {
    console.log(`Button clicked: ${action}`);
    // Add logic to handle different actions based on the button clicked
}

function consultDocument() {
    const documentNumber = document.getElementById('document-number').value;
    console.log(`Consulting document for number: ${documentNumber}`);
    // Add logic to consult the document
}

function registerAdditionalGuest() {
    const nationality = document.getElementById('additional-nationality').value;
    const documentType = document.getElementById('additional-document-type').value;
    const documentNumber = document.getElementById('additional-document-number').value;
    const firstSurname = document.getElementById('additional-first-surname').value;
    const secondSurname = document.getElementById('additional-second-surname').value;
    const names = document.getElementById('additional-names').value;

    console.log(`Registering additional guest: ${firstSurname} ${secondSurname}, ${names}`);
    // Add logic to register the additional guest
}