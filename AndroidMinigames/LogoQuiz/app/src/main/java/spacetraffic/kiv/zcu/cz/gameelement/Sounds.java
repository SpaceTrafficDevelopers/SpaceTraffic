/**
 Copyright 2010 FAV ZCU

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.

 **/
package spacetraffic.kiv.zcu.cz.gameelement;

import android.app.Activity;
import android.media.MediaPlayer;

import spacetraffic.kiv.zcu.cz.logoquiz.R;

/**
 * Sounds class.
 */
public class Sounds {

    /**
     * Medial player for wrong sound.
     */
    private MediaPlayer wrong;

    /**
     * Medial player for loss sound.
     */
    private MediaPlayer loss;

    /**
     * Medial player for correct sound.
     */
    private MediaPlayer correct;

    /**
     * Medial player for victory sound.
     */
    private MediaPlayer victory;

    /**
     * Medial player for finish sound.
     */
    private MediaPlayer finish;

    /**
     * Medial player for hurry sound.
     */
    private MediaPlayer hurry;

    /**
     * Medial player for start sound.
     */
    private MediaPlayer start;

    /**
     * Sounds constructor.
     * @param activity activity
     */
    public Sounds(Activity activity){
        this.wrong = MediaPlayer.create(activity, R.raw.missed);
        this.loss = MediaPlayer.create(activity, R.raw.stupid);
        this.correct = MediaPlayer.create(activity, R.raw.perfect);
        this.victory = MediaPlayer.create(activity, R.raw.victory);
        this.finish = MediaPlayer.create(activity, R.raw.nooo);
        this.hurry = MediaPlayer.create(activity, R.raw.hurry);
        this.start = MediaPlayer.create(activity, R.raw.hello);
    }

    /**
     * Method for play wrong sound.
     */
    public void playWrong() {
        this.wrong.start();
    }

    /**
     * Method for play loss sound.
     */
    public void playLoss() {
        this.loss.start();
    }

    /**
     * Method for play correct sound.
     */
    public void playCorrect() {
        this.correct.start();
    }

    /**
     * Method for play victory sound.
     */
    public void playVictory() {
        this.victory.start();
    }

    /**
     * Method for play finished sound.
     */
    public void playFinished() {
        this.finish.start();
    }

    /**
     * Method for play hurry sound.
     */
    public void playHurry() {
        this.hurry.start();
    }

    /**
     * Method for play start sound.
     */
    public void playStart() {
        this.start.start();
    }
}
