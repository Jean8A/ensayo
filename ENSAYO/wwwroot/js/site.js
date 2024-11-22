
    document.addEventListener('DOMContentLoaded', function () {
            const containers = document.querySelectorAll('.image-container');

            containers.forEach(container => {
                const images = container.querySelectorAll('.secondary-image');
    let index = 0;

                container.addEventListener('mouseover', () => {
                    if (images.length > 0) {
                        const interval = setInterval(() => {
        images.forEach((img, i) => {
            img.style.opacity = (i === index) ? '1' : '0';
        });
    index = (index + 1) % images.length;
                        }, 1000);

                        container.addEventListener('mouseleave', () => {
        clearInterval(interval);
                            images.forEach(img => img.style.opacity = '0'); // Reset to hide all
                        });
                    }
                });
            });
    });





