define 'room_users_view', ['general_view'] , (GeneralView) ->
    
    class RoomUsersView extends GeneralView
        id: 'room-users-container'
        initialize: ->
            console.log @model
            @data = @model.attributes
        render: ->
            get_user_field = (user, field_name) ->
                for field in user.data
                    if field.name == field_name
                        return field.value
            @$el.empty()
            @$el.attr 'class', 'span4 scroll-box'
            nav = $('<ul>').attr({'class':'nav nav-tabs nav-stacked'})
            nav.append "<li class='nav-header'>Room users</li>"
            if @model.has('members') and @data.members.length > 0
                for member in @data.members
                    #user_id = get_user_field(member,'id')
                    user_name = get_user_field(member,'name')
                    nav.append "<li><a >#{user_name}</a></li>"
            else
                nav.append "<li>There's currently no other user in this room</li>"
            @$el.append nav
            @$el.append "</ul>"
            @
        append_to: (parent) ->
            @$el.appendTo parent
