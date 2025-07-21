
function startClock() {
    function updateTime() {
        const hourHand = document.getElementById('hour-hand');
        const minuteHand = document.getElementById('minute-hand');
        const secondHand = document.getElementById('second-hand');

        // Ensure elements are found before proceeding
        if (!hourHand || !minuteHand || !secondHand) {
            console.error("Clock elements not found in the DOM.");
            return;
        }

        const today = new Date();
        const hours = today.getHours();
        const minutes = today.getMinutes();
        const seconds = today.getSeconds();

        const hourDegrees = ((hours % 12) / 12) * 360 + (minutes / 60) * 30; // 12-hour clock
        const minuteDegrees = (minutes / 60) * 360;
        const secondDegrees = (seconds / 60) * 360;

        // Update the clock hands' rotation
        hourHand.style.transform = `rotate(${hourDegrees}deg)`;
        minuteHand.style.transform = `rotate(${minuteDegrees}deg)`;
        secondHand.style.transform = `rotate(${secondDegrees}deg)`;

        setTimeout(updateTime, 1000); // Call every second
    }

    updateTime(); // Start the initial clock update
}