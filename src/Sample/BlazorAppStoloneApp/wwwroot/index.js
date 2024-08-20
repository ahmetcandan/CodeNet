async function fetchData(url, method, body, headers) {
    try {
        var response = await fetch(url, {
            method: method,
            body: body,
            headers: headers
        });
        return response;
    } catch (error) {
        console.error('There was an error fetching the data:', error);
        return error;
    }
}