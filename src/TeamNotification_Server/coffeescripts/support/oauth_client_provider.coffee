config = require('../config')()
OAuth= require('oauth').OAuth

bitbucket_oauth_client = ()->
     new OAuth "https://bitbucket.org/!api/1.0/oauth/request_token",
              "https://bitbucket.org/!api/1.0/oauth/access_token",
              config.bitbucket.key,
              config.bitbucket.secret,
              "1.0",
              "#{config.site.surl}/bitbucket/oauth/callback",
              "HMAC-SHA1"

module.exports = 
    bitbucket_oauth_client:bitbucket_oauth_client
