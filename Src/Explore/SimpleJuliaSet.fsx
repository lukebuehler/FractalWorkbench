
//Using complex numbers, the process iterated to generate the Mandelbrot set and the Julia sets takes a very simple form:
// z → z^2 + c
//where z and c are complex numbers.

//in general
// zn+1 = zn^2 + c

//Think of the complex number z as a pair (x, y) of real numbers, and think of the complex number c as a pair of real numbers (a, b).
//In these terms, z → z^2 + c becomes

// x → x^2 - y^2 + a
// y → 2⋅x⋅y + b

open System
open Microsoft.FSharp.Core.Operators
open System.Drawing

type c = {a:double; b:double}
type z = {x:double; y:double}

let nextZ z c =
    let nextX = (pown z.x 2) - (pown z.y 2) + c.a
    let nextY = 2.0*z.x*z.y + c.b
    {x = nextX; y = nextY}

//test
// https://classes.yale.edu/fractals/MandelSet/ComplexIteration/ComplexIterEx.html
let c = {a=1.0/2.0; b=(-1.0)/2.0}
let z0 = {x=0.0;y=0.0}
let z1 = nextZ z0 c
let z2 = nextZ z1 c
let z3 = nextZ z2 c

//distance from origin
// https://classes.yale.edu/fractals/MandelSet/JuliaSets/JSetComps2.html
// when distance from origin for zj > 2.0 we have the "escape criterion"
//  use: sqrt(x^2 + y^2)
let distanceFromOrigin z = sqrt ((pown z.x 2) + (pown z.y 2))
let escapeCriterion z = distanceFromOrigin z > 2.0

escapeCriterion {x = 0.1187; y = 1.493} //false
escapeCriterion {x = -2.4640; y = 0.6066} //true

let countIterations z c N =
    let rec countInner z c N i =
        if i >= N then i
        else
            if escapeCriterion z then i
            else countInner (nextZ z c) c N (i+1)
    countInner z c N 0
      

countIterations {x=0.5; y=0.7} {a=(-0.25); b=0.25} 10       


//If all of z0, ..., zN lie within a distance of 2 from the origin, we assert the sequence will never run away to infinity and so z0 belongs to Kc.

//set the range we want to investivate between -2 and 2
//let w = -2.0, 2.0
//let h = -2.0, 2.0
let w = -0.1, 0.1
let h = -0.1, 0.1

let width = 10000;
let height = 10000;

let c2 = {a=(-0.70176); b=(-0.3842)}
let N = 50


let bmp = new Bitmap(width, height)

for x in 0 .. bmp.Width - 1 do
  for y in 0 .. bmp.Height - 1 do 
    let x' = (float x / (float)width * (snd w - fst w)) + fst w
    let y' = (float y / (float)height * (snd h - fst h)) + fst h
    let it = countIterations {x=x';y=y'} c2 N
    let color = if it = N then Color.Black else Color.White
    bmp.SetPixel(x, y, color)

bmp.Save(@"F:\Tmp\FractalWorkbench\FractalWorkbench\fractal.png", Imaging.ImageFormat.Png)




//more: http://tomasp.net/blog/2014/japan-advent-art-en/
