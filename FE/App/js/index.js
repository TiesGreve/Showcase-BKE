const consentKey = 'gdpr-consent';
const consent = localStorage.getItem(consentKey);

const gdprConsent = document.getElementById('gdpr-consent');
const acceptBtn = document.getElementById('accept-btn');
const rejectBtn = document.getElementById('reject-btn');
const appContent = document.getElementById('app-content');

function blockInteraction() {
    gdprConsent.style.display = 'flex';
    document.body.style.overflow = 'hidden';
    appContent.style.pointerEvents = 'none';
    appContent.style.userSelect = 'none';
    appContent.style.filter = 'blur(3px)';
}

function unblockInteraction() {
    gdprConsent.style.display = 'none';
    document.body.style.overflow = '';
    appContent.style.pointerEvents = '';
    appContent.style.userSelect = '';
    appContent.style.filter = '';
}

function setConsent(value) {
    localStorage.setItem(consentKey, value);
    unblockInteraction();
    if(value === 'accepted') {
    console.log("User accepted cookies.");
    } else {
    console.log("User rejected cookies.");
    }
}

if (!consent) {
    blockInteraction();
} else {
    console.log(`Consent previously set: ${consent}`);
    unblockInteraction();
}

acceptBtn.addEventListener('click', () => setConsent('accepted'));
rejectBtn.addEventListener('click', () => setConsent('rejected'));