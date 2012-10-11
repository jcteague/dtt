class UsersCollection

    constructor: (@users) ->

    to_json: ->
        links = ({"name": user.email, "rel": "User", "href": "/user/#{user.id}"} for user in @users)
        get_data_for = (user) ->
            return {
                "href": "/user/#{user.id}"
                "data": [
                    {"name": "id", "value": user.id}
                    {"name": "email", "value": user.email}
                ]
            }

        return {
            users: (get_data_for user for user in @users)
            links:  [{"name":"self", "rel": "Users", "href": "/users/query"}].concat(links)
        }

    ###
    # return {
    #   self: "current_href"
    #   links: [
    #       {
    #           name: "name"
    #           rel: "rel"
    #           href: "href"
    #       }
    #   ],
    #   users: [
    #       {
    #           id: "id"
    #           name: "name"
    #           href: "href",
    #       },
    #       {
    #           id: "id"
    #           name: "name"
    #           href: "href",
    #       }
    #   ],
    #   rooms: [
    #       {
    #           id: "id"
    #           name: "name"
    #           owner_id: "owner_id"
    #           room_key: "room_key"
    #           href: "href"
    #       },
    #       {
    #           id: "id"
    #           name: "name"
    #           owner_id: "owner_id"
    #           room_key: "room_key"
    #           href: "href"
    #       }
    #   ],
    #   messages: [
    #       {
    #           user_id: "user_id"
    #           name: "name"
    #           body: "body"
    #           datetime: "datetime"
    #           stamp: "stamp"
    #       },
    #       {
    #           user_id: "user_id"
    #           name: "name"
    #           body: "body"
    #           datetime: "datetime"
    #           stamp: "stamp"
    #       }
    #   ],
    #   invitations: [
    #       {
    #           email: "email"
    #           chat_room_id: "chat_room_id"
    #           chat_room_name: "chat_room_name"
    #           date: "date"
    #           accepted: "accepted"
    #       },
    #       {
    #           email: "email"
    #           chat_room_id: "chat_room_id"
    #           chat_room_name: "chat_room_name"
    #           date: "date"
    #           accepted: "accepted"
    #       }
    #   ]
    #
    #
    #
    #
    #   template: {
    #       type: "type"
    #       data: [
    #           
    #       ]
    #           id: {
    #               type: "type"
    #               value: "value"
    #               rules: [
    #                   
    #               ]
    #           },
    #           name: {
    #               type: "type"
    #               value: "value"
    #               rules: [
    #                   
    #               ]
    #           }
    #       }
    #   }    

    fetch_to: (callback) ->
        Q.when(@collection, callback)

module.exports = UsersCollection

