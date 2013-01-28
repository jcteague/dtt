Repository = require('../repository')
https = require('https')
Q = require('q')
chat_room_by_owner_id_strategy = require('./chat_room_by_owner_id_strategy') 

get_repositories = (oa, oauth_access_token, oauth_access_token_secret)->
    deferred = Q.defer()
    get_repositories_callback = (error, data, response)->
        console.log data
        Q.resolve()
    oa.getProtectedResource "https://api.bitbucket.org/1.0/user", "GET", oauth_access_token, oauth_access_token_secret, get_repositories_callback
    return deferred.promise

strategy = (parameters) ->
    Q.all([chat_room_by_owner_id_strategy({owner_id:parameters.user_id}), get_repositories(parameters.oa, parameters.oauth_access_token, parameters.oauth_access_token_secret)]).spread (user_rooms, response)->
    
        get_rep_fields = (repo) ->
            return {id:repo.id, owner:repo.owner, name:repo.name, description:repo.description, url:repo.url }
        
        console.log response
            
        return { success:false, messages:["done"] }
        if(response.success)
            repositories_array = []
            for rep in response.repositories
                repositories_array.push get_rep_fields(rep)
            { success:true, rooms:user_rooms.rooms, access_token:parameters.oauth_access_token, repositories:repositories_array  }
        else
            { success:false, messages:repositories.messages }
        
        
module.exports = strategy
