/**
 * Set of functions for graphic transformation calculations.
 * It uses array as matrix representation.
 * It uses only 3x3 matrices in following format:
 * [ a b c ]
 * [ d e f ]  => [a b c d e f g h i ]
 * [ g h i ]
 */
var GT2D = {
    /**
     * Rotate
     */
    rotate: function(deg)
    {
        var rad = parseFloat(deg) * (Math.PI/180.0),
            costheta = Math.cos(rad),
            sintheta = Math.sin(rad);

        var a = costheta,
            b = sintheta,
            c = -sintheta,
            d = costheta;

        return [  a,   c, 0.0,
                  b,   d, 0.0,
                0.0, 0.0, 1.0];
    },

    /**
     * Scale
     */
    scale: function (sx, sy)
    {
        sx = sx || sx === 0 ? sx : 1;
        sy = sy || sy === 0 ? sy : 1;
        return [sx, 0,  0,
          0,  sy, 0,
          0,  0,  1
        ];
    },

    /**
     * Scale, x axis
     */
    scaleX: function (sx)
    {
        return this.scale(sx);
    },

    /**
     * Scale, y axis
     */
    scaleY: function (sy)
    {
        return this.scale(1, sy);
    },

    /**
     * Skew (zkosení), in deg
     */
    skew: function (degX, degY)
    {
        var radX = parseFloat(degX) * (Math.PI/180),
            radY = parseFloat(degY) * (Math.PI/180),
            x = Math.tan(radX),
            y = Math.tan(radY);

        return [
          1, x, 0,
          y, 1, 0,
          0, 0, 1
        ];
    },

    /**
     * Skew, x axis
     */
    skewX: function (deg)
    {
        var rad = parseFloat(deg) * (Math.PI/180),
            x = Math.tan(rad);

        return [
          1, x, 0,
          0, 1, 0,
          0, 0, 1
        ];

    },

    /**
     * Skew, y axis
     */
    skewY: function (deg)
    {
        var rad = parseFloat(deg) * (Math.PI/180.0),
            y = Math.tan(rad);

        return [
          1.0, 0.0, 0.0,
          y, 1.0, 0.0,
          0.0, 0.0, 1.0
        ];

    },

    /**
     * Translation
     */
    translate: function (tx, ty)
    {
        tx = tx ? tx : 0.0;
        ty = ty ? ty : 0.0;

        return [
          1.0, 0.0, tx,
          0.0, 1.0, ty,
          0.0, 0.0, 1.0
        ];
    },

    /**
     * Translation, x axis
     */
    translateX: function (tx)
    {
        return this.translate(tx);
    },

    /**
     * Translation, y axis
     */
    translateY: function (ty)
    {
        return this.translate(0, ty);
    },

    /**
     * Generates array from matrix values in format for css transformations.
     * The format is:
     * [ a c x ]
     * [ b d y ]    => [ a, b, c, d, x, y ]
     * [ 0 0 1 ]
     */
    // Generuje pole odpovídající matici transformace pro potřeby nastavení v css.
    toCssMatrix: function(A)
    {
        return [A[0], A[3], A[1], A[4], A[2],A[5]];
    },

    /**
     * Same functionality as toCssMatrix, except it will round all values.
     */
    toCssMatrixFixed: function(A)
    {
        return [Math.round(A[0]), Math.round(A[3]), Math.round(A[1]), Math.round(A[4]), Math.round(A[2]), Math.round(A[5])];
    },
//     Vypíše matici transformace. Pro ladící účely.
//    toString: function(matrix) {
//        return '['+matrix.e(1,1)+', '+matrix.e(2,1)+', '+matrix.e(1,2)+', '+matrix.e(2,2)+', '+matrix.e(1,3)+', '+matrix.e(2,3)+']';
//    }

    /**
     * Matrix cross product of two matrices 3x3.
     */
    X: function (A, B) {
    	return [
    	        A[0]*B[0]+A[1]*B[3]+A[2]*B[6],
    	        A[0]*B[1]+A[1]*B[4]+A[2]*B[7],
    	        A[0]*B[2]+A[1]*B[5]+A[2]*B[8],
    	        A[3]*B[0]+A[4]*B[3]+A[5]*B[6],
    	        A[3]*B[1]+A[4]*B[4]+A[5]*B[7],
    	        A[3]*B[2]+A[4]*B[5]+A[5]*B[8],
    	        A[6]*B[0]+A[7]*B[3]+A[8]*B[6],
    	        A[6]*B[1]+A[7]*B[4]+A[8]*B[7],
    	        A[7]*B[2]+A[7]*B[5]+A[8]*B[8]
    	        ];
    },

    /**
     * Matrix cross product optimised for transformation matrices.
     * Requires matrix in format:
     * [ x x x ]
     * [ x x x ]
     * [ 0 0 1 ]
     */
    XFast: function (A, B) {
    	return [ //prvni radek
    	        A[0]*B[0]+A[1]*B[3],
    	        A[0]*B[1]+A[1]*B[4],
    	        A[0]*B[2]+A[1]*B[5]+A[2],

    	        //druhy rader
    	        A[3]*B[0]+A[4]*B[3],
    	        A[3]*B[1]+A[4]*B[4],
    	        A[3]*B[2]+A[4]*B[5]+A[5],

    	        //treti radek
    	        0,
    	        0,
    	        1
    	        ];
    },

    /**
     * Gets element on ith row and jth column in given matrix.
     */
    e: function (A, i, j) {
    	return A[Math.floor(i/3)+j-1];
    }
};


