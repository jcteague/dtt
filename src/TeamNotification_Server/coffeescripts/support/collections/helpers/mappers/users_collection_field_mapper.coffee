default_mapper = require('./default_collection_field_mapper.')

get_user_from = (user) ->
    return {
        "href": user.href
        "data": default_mapper.map(user)
    }

map = (users) ->
    (get_user_from user for user in users)

module.exports =
    map: map
