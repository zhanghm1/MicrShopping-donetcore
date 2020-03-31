var apiUrl=process.env.VUE_APP_apiUrl;
var identityUrl=process.env.VUE_APP_identityUrl;
var webVueUrl=process.env.VUE_APP_webVueUrl;

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

