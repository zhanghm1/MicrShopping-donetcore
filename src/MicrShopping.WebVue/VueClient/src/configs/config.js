var ApiUrl="http://192.168.0.189:5002";

var OidcConfig = {
    authority: "http://192.168.0.189:5008/",
    client_id: "js-vue",
    redirect_uri: "http://192.168.0.189:5015/#/callback",
    response_type: "code",
    scope:"openid profile orderapi productapi usermanageapi",
    post_logout_redirect_uri : "http://192.168.0.189:5015/",
};
export {
    ApiUrl,OidcConfig
} ;

