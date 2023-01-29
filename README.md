# Chinook

This application is unfinished. Please complete below tasks. Spend max 3 hours. We would like to have a short written 
explanation of the changes you made.

1. Move data retrieval methods to separate class / classes (use dependency injection)

	* Used dependency injection to inject Data access objects via ServiceCollection Extension method.

2. Favorite / unfavorite tracks. An automatic playlist should be created named "My favorite tracks"
	

3. The user's playlists should be listed in the left navbar. If a playlist is added (or modified), this should 
	reflect in the left navbar. There is already one playlist link in the Nav Menu as an example.

	* Added functionality to add new playlist and to populate users left nav with playlists.
	* "Add to playlist Modal" is used to add new playlist.

4. Add tracks to a playlist (existing or new one). The dialog is already created but not yet finished.

	* New playlist will be created if new paylist input box is not empty.
	* Implement logics to populate user's playlist dropdown of the modal.
	* Adding Track to playlist - newly created of selected.
	* BUG - user must change selection dropdown to system to identify selected playlist.

5. The user should be able to remove tracks from the playlist.
6. User should be able to rename the playlist


7. User should be able to remove the whole playlist


8. Search for artist name

	* Add new input field to the index.
	* Handle input event of search input field.
	* Pass search query to the data access layer and show filtered Artist list.
	* Search happens as user type the query.
	* Search is case-sensitive.

When creating a user account, you will see this:
"This app does not currently have a real email sender registered, see these docs for how to configure a real email sender. 
Normally this would be emailed: Click here to confirm your account."
After you click 'Click here to confirm your account' you should be able to login.
