define 'room_users_view', ['general_view'] , (GeneralView) ->
    
    class RoomUsersView extends GeneralView
        id: 'room-users-container'
        initialize: ->
            @data = @model.attributes
        render: ->
            get_user_field = (user, field_name) ->
                for field in user.data
                    if field.name == field_name
                        return field.value
            @$el.empty()
            @$el.attr 'class', 'span4 scroll-box'
            #@$el.attr("style","height:500px;")
            #@$el.append "<ul class='nav nav-list'>"
            nav = $('<ul>').attr({'class':'nav nav-tabs nav-stacked'})
            nav.append "<li class='nav-header'>Room users</li>"
            for member in @data.members
                user_id = get_user_field(member,'id')
                user_name = get_user_field(member,'name')
                nav.append "<li><a href='/#/user/#{user_id}'>#{user_name}</a></li>"
            @$el.append nav
            @$el.append "</ul>"
            @
        append_to: (parent) ->
            @$el.appendTo parent
