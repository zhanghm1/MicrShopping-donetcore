var apiUrl=process.env.VUE_APP_apiUrl;
var identityUrl=process.env.VUE_APP_identityUrl;
var webVueUrl=process.env.VUE_APP_webVueUrl;

console.log("NODE_ENV:"+process.env.NODE_ENV);
if(process.env.NODE_ENV=="development"){
    apiUrl='http://192.168.0.189:5002';
    identityUrl='http://192.168.0.189:5012';
    webVueUrl='http://192.168.0.189:5015';
}
console.log("apiUrl:"+apiUrl);
console.log("identityUrl:"+identityUrl);
console.log("webVueUrl:"+webVueUrl);


var OidcConfig = {
    authority: identityUrl,
    client_id: "js-vue",
    redirect_uri: webVueUrl+"/#/callback",
    response_type: "code",
    scope:"openid profile orderapi productapi usermanageapi",
    post_logout_redirect_uri : webVueUrl,
};
export {
    apiUrl,OidcConfig
} ;

