extends Node2D

@export var animated: AnimationPlayer
@export var animatedtw: AnimationPlayer

signal cutscene_finished

var is_skipping := false

func _ready():
	if PlayerDataTw.instance.inicio == 0:
		animatedtw.play("initial1")
		var player_node = $Player as CharacterBody2D
		if player_node:
			player_node.move_speed = 0
			player_node.jump_force = 0

		animatedtw.animation_finished.connect(_on_animation_finished)
		PlayerDataTw.instance.inicio = 1
	else:
		animated.play("light")


func _on_animation_finished(anim_name: StringName) -> void:
	print("Animação '%s' terminou." % anim_name)
	animatedtw.stop()
	animated.play("light")

	var player_node = $Player as CharacterBody2D
	if player_node:
		player_node.move_speed = 100
		player_node.jump_force = 300


func _process(delta: float) -> void:
	if Input.is_action_just_pressed("cancel"):
		if not is_skipping:
			is_skipping = true
			print("Animação pulada!")

			animatedtw.stop()
			emit_signal("cutscene_finished")
			animated.play("light")

			var player_node = $Player as CharacterBody2D
			if player_node:
				player_node.move_speed = 100
				player_node.jump_force = 300
