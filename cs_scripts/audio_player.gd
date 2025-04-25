extends AudioStreamPlayer

const level_music = preload("res://audio/song.mp3")

func _ready():
	connect("finished", Callable(self, "_on_music_finished"))
	play_music_level()  # Inicia a música assim que o jogo começar

func _play_music(music: AudioStream, volume = -20.0):
	if stream == music:
		return
	stream = music
	volume_db = volume
	play()

func play_music_level():
	_play_music(level_music)

func _on_music_finished():
	play()
