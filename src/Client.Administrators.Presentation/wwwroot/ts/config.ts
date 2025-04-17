let gatewayUrl: string | Record<string, unknown> = {};

fetch('/Configuration/Get')
    .then((response: Response) => response.json())
    .then((config: { gatewayUrl: string | Record<string, unknown> }) => {
        gatewayUrl = config.gatewayUrl;
        document.dispatchEvent(new Event("configLoaded"));
    })
    .catch((error: unknown) => {
        console.error("Error fetching configuration:", error);
    });