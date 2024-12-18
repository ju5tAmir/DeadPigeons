export function decodeJWT(token: string) {
    // Split the JWT into three parts
    const [headerB64, payloadB64, signatureB64] = token.split('.');

    // Decode the base64-encoded header and payload
    const decodedHeader = JSON.parse(atob(headerB64.replace(/-/g, '+').replace(/_/g, '/')));
    const decodedPayload = JSON.parse(atob(payloadB64.replace(/-/g, '+').replace(/_/g, '/')));

    // Return the decoded header and payload
    return {
        header: decodedHeader,
        payload: decodedPayload,
    };
}

export function getRoleFromJWT(jwt: string) {
    // Decode the JWT using the existing decodeJWT function
    const decodedPayload = decodeJWT(jwt);

    // Access the roles claim
    return decodedPayload.payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
}
