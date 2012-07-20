define 'user_rooms_view', ['general_view','messages_view','form_view','form_template_renderer'], (GeneralView, MessagesView, FormView, FormTemplateRenderer) ->

    class UserRoomsView extends GeneralView
        
        id: 'user-rooms-container'
        initialize: ->
            @formTemplateRenderer = new FormTemplateRenderer()
            @data = @model.attributes
        render: ->
            @$el.empty()
            @$el.attr( 'class', 'well inline')
            @$el.append "<a href='/client#/user/#{@data.user.user_id}/' class='span4'>#{@data.user.name}</a>"
            userRooms = @formTemplateRenderer.dropDownListBuilder({name:'room', label:'Rooms'})
            $(userRooms[1]).bind "change", ()->
                window.location.assign($(userRooms[1]).val())
            for room in @data.user_rooms
                roomOption = @formTemplateRenderer.dropDownListOptionBuilder({value:"client#/room/#{room.room_id}/messages",text:room.name})
                if ("/room/#{room.room_id}/messages" == @data.href)
                    roomOption.attr('selected', 'selected')
                userRooms[1].append roomOption
            @$el.append userRooms[1]
            @
        append_to: (parent) ->
            @$el.appendTo parent
