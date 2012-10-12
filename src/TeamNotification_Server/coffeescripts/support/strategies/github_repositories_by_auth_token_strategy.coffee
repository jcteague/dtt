Repository = require('../repository')
https = require('https')
Q = require('q')
chat_room_by_owner_id_strategy = require('./chat_room_by_owner_id_strategy') 

get_repositories = (access_token) ->
    options =
        host: "api.github.com"
        path: "/user/repos?access_token=#{access_token}"
        port: 443
        method: 'GET'
    deferred = Q.defer()
    repositories_response_parts = []
    repositories_string = ""
    req = https.request options, (res) ->
        res.setEncoding('utf8')
        res.on 'data', (repositories) ->
            repositories_response_parts.push(repositories)
            #console.log repositories
            #deferred.resolve({success:true, repositories:JSON.parse(repositories)})
        res.on 'end', () ->
            repositories_string = repositories_response_parts.join('')        
            console.log( repositories_response_parts.join('') )
            deferred.resolve({success:true, repositories:JSON.parse(repositories_string)})
        res.on 'error', (err) ->
            console.log err
            deferred.resolve({success:false, messages:[err.message], repositories:null})
    req.end()
    deferred.promise
    


strategy = (parameters) ->
    Q.all([chat_room_by_owner_id_strategy({owner_id:parameters.user_id}), get_repositories(parameters.access_token)]).spread (user_rooms, response) ->
        get_rep_fields = (repo) ->
            return {id:repo.id, owner:repo.owner, name:repo.name, description:repo.description, url:repo.url }
            
        if(response.success)
            repositories_array = []
            for rep in response.repositories
                repositories_array.push get_rep_fields(rep)
            { success:true, rooms:user_rooms.rooms, access_token:parameters.access_token, repositories:repositories_array  }
        else
            { success:false, messages:repositories.messages }
        
        
module.exports = strategy
