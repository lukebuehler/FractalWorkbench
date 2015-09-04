//-------------------------
// Simple tutorial that explores Mandelbrot sets as introduced on the Yale Fractals site
// https://classes.yale.edu/fractals/MandelSet/welcome.html
//-------------------------

//make sure you understand Julia sets first

//https://classes.yale.edu/fractals/MandelSet/MandelDef/Definition/Definition.html
//We use the same process as Julia sets but where before c was a constant and z was our input, we do test c as our input now.
// we can say Julia sets are on the Dynamical Plane (z) and Mandelbrot sets are on the Parameter Plance (c)

// in the sequence z starts as 0+i0 here.
// z → z^2 + c

//in general
// zn+1 = zn^2 + c

//before, we used our own method to calculate the c & z now we use the F# complex struct

open System
open Microsoft.FSharp.Core.Operators
open System.Drawing
open System.Numerics

let next (z:Complex) (c:Complex) = z * z + c
let willEscape (z:Complex) = z.Magnitude > 2.0

let countIterations z c N =
    let rec countInner z c N i =
        if i >= N then i
        else
            if willEscape z then i
            else countInner (next z c) c N (i+1)
    countInner z c N 0
      
let countMandelIterations c N = countIterations (Complex(0.0,0.0)) c N

//set the range we want to investivate between -2 and 2
let w = -2.0, 2.0
let h = -2.0, 2.0

//how many pixels?
let width = 10000;
let height = 10000;

let N = 50 //max iterations

//create image
//for now, we'll just color all points that do not run away to infinity black (Kc) and the others white
// the way we assume it does not run away is if the iterations are equal to N (max iterations)
let bmp = new Bitmap(width, height)
for x in 0 .. bmp.Width - 1 do
  for y in 0 .. bmp.Height - 1 do 
    let x' = (float x / (float)width * (snd w - fst w)) + fst w
    let y' = (float y / (float)height * (snd h - fst h)) + fst h
    let it = countMandelIterations (Complex(x', y')) N
    let color = if it = N then Color.Black else Color.White
    bmp.SetPixel(x, y, color)

bmp.Save(@"C:\Users\lbuhler\Desktop\Fractals\mandelbrot.png", Imaging.ImageFormat.Png)


//coloring from here: http://tomasp.net/blog/2014/japan-advent-art-en/
// Transition between colors in 'count' steps
let (--) clr count = clr, count
let (-->) ((r1,g1,b1), count) (r2,g2,b2) = [
  for c in 0 .. count - 1 ->
    let k = float c / float count
    let mid v1 v2 = 
      int (float v1 + ((float v2) - (float v1)) * k) 
    Color.FromArgb(mid r1 r2, mid g1 g2, mid b1 b2) ]
//dp the same colored
let palette = 
  [| // 3x sky color & transition to light blue
     yield! (245,219,184) --3--> (245,219,184) 
     yield! (245,219,184) --4--> (138,173,179)
     // to dark blue and then medium dark blue
     yield! (138,173,179) --4--> (2,12,74)
     yield! (2,12,74)     --4--> (61,102,130)
     // to wave color, then light blue & back to wave
     yield! (61,102,130)  -- 8--> (249,243,221) 
     yield! (249,243,221) --32--> (138,173,179) 
     yield! (138,173,179) --32--> (61,102,130) |]

let bmp2 = new Bitmap(width, height)
for x in 0 .. bmp2.Width - 1 do
  for y in 0 .. bmp2.Height - 1 do 
    let x' = (float x / (float)width * (snd w - fst w)) + fst w
    let y' = (float y / (float)height * (snd h - fst h)) + fst h
    let it = countMandelIterations (Complex(x', y')) palette.Length
    let color = palette.[it-1]
    bmp2.SetPixel(x, y, color)

bmp2.Save(@"C:\Users\lbuhler\Desktop\Fractals\mandelbrot-color.png", Imaging.ImageFormat.Png)

