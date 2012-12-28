class RoomMembersCollectionBuilder
    constructor: (@href) ->
    
    get: ()->
        return [{
          "href" : "/users/query"
          "rel" : "users"
          "prompt" : "Enter search string"
          "type" : "autocomplete"
          "submit": @href
          "data" :[{"name" : "email", "value" : ""}]
        }]


module.exports = RoomMembersCollectionBuilder
