;------------------------------------------------------------------------------------------------------
; 1. bounce-into-bucket
;------------------------------------------------------------------------------------------------------

(define (game bounce-into-bucket) (:domain standard-five-objects)
    (:setup
        (and
            (exists (?x - box) 
                ; using a grid system of 9 blocks --> # | but it is not precise enough?
                (game-conserved (position ?x top-center)))
            (exists (?U - bucket) 
                ; need to make sure player does not move bucket after shot
                (game-conserved (position ?U bottom-left)))
        )        
    )
    (:constraints
        (preference deflectedThrow
            ; use 3 args in either?
            (exists (?b - ball ?x - box ?U - bucket ?o - (either gear triangle corner))
                (then
                    (once (ball_shot ?b))
                    ; somehow avoid player using other objects
                    (hold (and (in_motion ?b) (not (touch ?o ?b) (not (touch ?U ?b)))) 
                    (once (touch ?x ?b))
                    (hold (and (in_motion ?b) (not (touch ?o ?b)) (not (touch ?x ?b))))
                    (once (and (in ?U ?b) (not (in_motion ?b))))
                )
            )
        )
    )
    (:scoring maximize (count-nonoverlapping deflectedThrow))
)

;------------------------------------------------------------------------------------------------------
; 2. shoot-through-gaps
;------------------------------------------------------------------------------------------------------

(define (game shoot-through-gaps) (:domain standard-five-objects)
    (:setup
        (exists (?x - box ?U - bucket ?b - ball ?t - triangle ?c - corner) 
            (game-conserved
                (and
                    (position ?t top-left)
                    ; right order
                    (below ?t ?c)
                    (below ?c ?x)
                    ; in line
                    (equal_x_position ?t ?c)
                    (equal_x_position ?c ?x)
                    ; enough space for ball to pass
                    (>= (distance_side ?t bottom ?c top) (size ?b))
                    (>= (distance_side ?c bottom ?x top) (size ?b))
                    ; bucket to left
                    (>= (distance_side ?U right ?t left) 0)
                )
            )
        )    
    )
    (:constraints
        (and
            (preference topThrow
                (exists (?b - ball ?U - bucket ?t - triangle)
                    (then
                        (once (ball_shot ?b))
                        (hold-while (in_motion ?b)
                                    (above ?t ?b))
                        (once (and (in ?U ?b) (not (in_motion ?b))))
                    )
                )
            )
            (preference midThrow
                (exists (?b - ball ?U - bucket ?t - triangle ?c - corner ?x - box)
                    (then
                        (once (ball_shot ?b))
                        (hold-while (in_motion ?b)
                            ; one of the middle gaps
                            (or 
                                (and (above ?c ?b) (below ?t ?b))
                                (and (above ?x ?b) (below ?c ?b))
                            ))
                        (once (and (in ?U ?b) (not (in_motion ?b))))
                    )
                )
            )
            (preference bottomThrow
                (exists (?b - ball ?U - bucket ?x - box)
                    (then
                        (once (ball_shot ?b))
                        (hold-while (in_motion ?b)
                                    (below ?x ?b))
                        (once (and (in ?U ?b) (not (in_motion ?b))))
                    )
                )
            )
        )
    )
    (:scoring maximize 
        (+  (* 3 (count-nonoverlapping topThrow))
            (* 2 (count-nonoverlapping midThrow))
            (* 1 (count-nonoverlapping bottomThrow))
        )
    )
)


;------------------------------------------------------------------------------------------------------
; 3. random-timed-shoot
;------------------------------------------------------------------------------------------------------

(define (game random-timed-shoot) (:domain standard-five-objects)
    (:setup
        (exists (?U - bucket ?b - ball) 
            ;TODO: somehow random appearances? Probably not in setup?
        )   
    )
    (:constraints
        (preference successfulThrow
        ; TODO: Somehow time this - maybe compare to value in a clock object?
            (exists (?b - ball ?U - bucket)
                (then
                    (once (ball_shot ?b))
                    (hold (in_motion ?b)) 
                    (once (and (in ?U ?b) (not (in_motion ?b))))
                )
            )
        )
    )
    (:scoring maximize (count-nonoverlapping successfulThrow))
)

;------------------------------------------------------------------------------------------------------
; 4. platform-end-state
;------------------------------------------------------------------------------------------------------

(define (game platform-end-state) (:domain standard-five-objects)
    (:setup
        ; more obstacles will be places in different levels of this game? Or should that be part of the
        ; same problem definition?
        (exists (?x - box ?c - corner ?b - ball)
            (game-conserved 
                ; trying positions as x-y positions on a unit scale in the environment squared area 
                (position ?x 0.9375 0.6)
                (position ?c 1 0.6)
            )
        )   
    )
    (:constraints
        (preference successfulThrow
            (exists (?x - box ?c - corner ?b - ball)
                (then
                    (once (ball_shot ?b))
                    (hold (in_motion ?b)) 
                    (once   
                        (and 
                            (or (on ?x ?b) (on ?c ?b)) 
                            (not (in_motion ?b))
                        )
                    )
                )
            )
        )
    )
    (:scoring maximize (count-nonoverlapping successfulThrow))
)

;------------------------------------------------------------------------------------------------------
; 5. rest-in-gap
;------------------------------------------------------------------------------------------------------

(define (game rest-in-gap) (:domain standard-five-objects)
    (:setup
    )
    (:constraints
    )
    (:scoring
    )
)

;------------------------------------------------------------------------------------------------------
; 6. triangle-as-division
;------------------------------------------------------------------------------------------------------

(define (game triangle-as-division) (:domain standard-five-objects)
    ; how to code up increasing obstacles for future levels?
    (:setup
        (exists (?t - triangle)
            (game-conserved
                (and
                    ; use floor as a domain-wide variable somehow or use it as a typed object??
                    (touch ?t floor)
                    ; just because game-conserved in setup, doesn't mean applies to every state?
                    (< (x_position ?t) (x_position ?b))
                )
            )
        )
    )
    (:constraints
        (preference successfulThrow
            (exists (?t - triangle ?b - ball)
                (then
                    (once (ball_shot ?b))
                    (hold (in_motion ?b)) 
                    (once   
                        (and 
                            ; could use that distance side thing here too?
                            (> (x_position ?t) (x_position ?b))
                            (not (in_motion ?b))
                        )
                    )
                )
            )
        )
    )
    (:scoring maximize (count-nonoverlapping successfulThrow))
)


;------------------------------------------------------------------------------------------------------
; 7. stay-on-box
;------------------------------------------------------------------------------------------------------

(define (game stay-on-box) (:domain standard-five-objects)
    (:setup
        (exists (;TODO)
            (game-conserved
                ;TODO
            )
        )
    )
    (:constraints
        (preference successfulThrow
            (exists (?t - triangle ?b - ball)
                (then
                    (once (ball_shot ?b))
                    (hold (in_motion ?b)) 
                    (once   
                        ;TODO
                    )
                )
            )
        )
    )
    (:scoring maximize (count-nonoverlapping successfulThrow))
)

;------------------------------------------------------------------------------------------------------
; 8. touch-as-many
;------------------------------------------------------------------------------------------------------

(define (game touch-as-many) (:domain standard-five-objects)
    (:setup
        (exists (?b - ball ?x - box ?t - triangle ?g - gear ?c - corner)
            ;TODO: Random or arbitrary placement of objects
        )
    )
    (:constraints
        (preference touchedObject
            (exists (?o - game_object ?b - ball)
                (then
                    (once (ball_shot ?b))
                    (hold-while (in_motion ?b) 
                        (touch ?o ?b))
                    (once (not (in_motion ?b)))
                )
            )
        )
    )
    ; how does this function know which object to count and maximize for? will it count the ball?
    (:scoring maximize (/
                            (*  (count-once-per-objects touchedObject) 
                                (+ (count-once-per-objects touchedObject) 1)
                            )
                        ; n(n+1)/2 for nth partial sum series
                        2
                        )
    )
)


;------------------------------------------------------------------------------------------------------
; 9. touch-as-many-shoot
;------------------------------------------------------------------------------------------------------

(define (game touch-as-many-shoot) (:domain standard-five-objects)
    (:setup
        (exists (?b - ball ?x - box ?t - triangle ?g - gear ?c - corner ?U - bucket)
            ;TODO: Random or arbitrary placement of objects
        )
    )
    (:constraints
        (preference touchedObjectSuccessfulThrow
            ; walls should not count as game objects...
            (exists (?o - game_object ?b - ball ?U - bucket)
                (then
                    (once (ball_shot ?b))
                    (hold-while (in_motion ?b) 
                        (touch ?o ?b))
                    (once (and (not (in_motion ?b)) (in ?U ?b)))
                )
            )
        )
    )
    (:scoring maximize (count-once-per-objects touchedObjectSuccessfulThrow))
)

;------------------------------------------------------------------------------------------------------
; 10. three-wall-touch-shoot
;------------------------------------------------------------------------------------------------------

(define (game three-wall-touch-shoot) (:domain standard-five-objects)
    (:setup
        (exists (?b - ball ?U - bucket)
           (game-optional (position ?U bottom-left))
        )
    )
    (:constraints
        (preference successfulThrow
            (exists (?b - ball ?U - bucket)
                (then
                    (once (ball_shot ?b))
                    ; does this hold while imply order in predicates? 
                    ; only want "at least one state to satisfy the following predicates"
                    (hold-while (in_motion ?b) 
                        (touch left_wall ?b)
                        (touch roof ?b)
                        (touch right_wall ?b))
                    (once (and (not (in_motion ?b)) (in ?U ?b)))
                )
            )
        )
    )
    (:scoring maximize (count-nonoverlapping successfulThrow))
)


;------------------------------------------------------------------------------------------------------
; 11. count-wall-touches
;------------------------------------------------------------------------------------------------------

(define (game count-wall-touches) (:domain standard-five-objects)
    (:setup
    )
    (:constraints
        (preference countTouches
            (exists (?b - ball)
                (then
                    (once (ball_shot ?b))
                    ; count only when left or right wall is touched
                    (hold-while (and (in_motion ?b) (not (touch floor ?b)))
                        (touch left_wall ?b)
                        (touch right_wall ?b))
                    ; end on floor touch
                    (once (touch floor ?b))
                )
            )
        )
    )
    ; touches don't count as objects... 
    ; so we count every non-overlapping instance of the whole preference
    ; but how do we keep track of the touches INSIDE each preference? 
    (:scoring maximize (count-nonoverlapping countTouches))
)


;------------------------------------------------------------------------------------------------------
; 12. balance-ball-into-bucket
;------------------------------------------------------------------------------------------------------

(define (game balance-ball-into-bucket) (:domain standard-five-objects)
    (:setup
    ; TODO: bucket is placed somewhere high up
    )
    (:constraints (and
        ; does putting this preference here imply it should be executed at least once?
        (preference freeBall
            ; maybe the ball_shot predicate doesn't even need an arg because there is only one ball...
            ; is the 'exists' even needed here? 
            (once ball_shot)
        )
        (preference balanceBall
            (exists (?b - ball ?g - gear ?o - (either gear corner triangle))
                (then
                ; ball doesn't need to be shot for the game itself. It only needs to be 
                ; in shoot mode aka free to move.
                ; player only needs to free it once, right in the start...
                    (once (touch ?g ?b))
                    ; this is tricky because its not required to ALWAYS touch the ball with the gear - 
                    ; it can also be in free fall
                    ; maybe this hold requirement is enough constraint?
                    (hold (not (touch ?o ?b)))
                    (once (and (in ?U ?b) (not (in_motion ?b))))
                )
            )
        )
        )
    )
    ; touches don't count as objects... 
    ; so we count every non-overlapping instance of the whole preference
    ; but how do we keep track of the touches INSIDE each preference? 
    (:scoring maximize (count-nonoverlapping balanceBall))
)


;------------------------------------------------------------------------------------------------------
; 12. constant-interaction-no-fall
;------------------------------------------------------------------------------------------------------

(define (game constant-interaction-no-fall) (:domain standard-five-objects)
    (:setup
    ; TODO: objects are placed around the pedestal
    )
    (:constraints 
        (preference noBallFall
            (exists (?b - ball ?o - game-object)
                (then
                    (once (ball_shot ?b))
                    ; added no touching of floor inside the hold condition
                    (hold-while (and (in_motion ?b) (not (touch floor ?b)))
                        ; touch any other object
                        (touch ?o ?b)
                    )
                    (once (touch floor ?b))
                )
            )
        )
    )
    ; player would get points even if they did not keep interacting with objects and made a good shot...
    (:scoring maximize (count-once-per-objects noBallFall))
)