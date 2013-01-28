Repository = require('../repository')
https = require('https')
Q = require('q')
chat_room_by_owner_id_strategy = require('./chat_room_by_owner_id_strategy') 
get_oauth_client = require('../../support/oauth_client_provider').bitbucket_oauth_client

get_repositories = (oauth_access_token, oauth_access_token_secret)->
    deferred = Q.defer()
    oa = get_oauth_client()
    get_repositories_callback = (error, data, response)->
        console.log data
        deferred.resolve({success:true, repositories:JSON.parse(data).repositories})
    oa.getProtectedResource "https://api.bitbucket.org/1.0/user", "GET", oauth_access_token, oauth_access_token_secret, get_repositories_callback
    deferred.promise

strategy = (parameters) ->
    Q.all([chat_room_by_owner_id_strategy({owner_id:parameters.user_id}), get_repositories(parameters.oauth_access_token, parameters.oauth_access_token_secret)]).spread (user_rooms, response)->
        get_rep_fields = (repo) ->
            return {owner:{ login: repo.owner}, name:repo.name, description:repo.description, url:repo.resource_uri }
            
        if(response.success)
            repositories_array = []
            for rep in response.repositories
                repositories_array.push get_rep_fields(rep)
            return { success:true, rooms:user_rooms.rooms, oauth_token:parameters.oauth_access_token, oauth_token_secret:parameters.oauth_access_token_secret,repositories:repositories_array }
        else
            return { success:false, messages:repositories.messages }

module.exports = strategy
