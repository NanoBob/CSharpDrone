
import hmacSHA256 from "crypto-js/hmac-sha256";
import Base64 from 'crypto-js/enc-base64';
import { config } from "../config"

function getHash(timestamp: number, key: string) {
    var unixtimeString = timestamp.toString()

    const hmac = hmacSHA256(unixtimeString, key)
    const stringified = Base64.stringify(hmac);
    return stringified;
}

export function getAuthenticationHash(key: string) {
    const unixtime = ~~(+new Date() / 1000);
    const roundedTime = unixtime - (unixtime % config.authenticationTimeSpan);
    return getHash(roundedTime, key);
}