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
package spacetraffic.kiv.zcu.cz.logoquiz;

import android.os.CountDownTimer;
import android.widget.TextView;

import spacetraffic.kiv.zcu.cz.gameelement.Sounds;

/**
 * My countdown timer class.
 */
public class MyCountDownTimer {

    /**
     * Millisecond until timer finished.
     */
    private long millisUntilFinished = -1;

    /**
     * Start time.
     */
    private long start;

    /**
     * Tick interval.
     */
    private long interval;

    /**
     * Reference on countdown timer.
     */
    private CountDownTimer cdt;

    /**
     * Updating text view.
     */
    private TextView tv;

    /**
     * String with remain message.
     */
    private String remain;

    /**
     * On finish callback action.
     */
    private IPostAction postAction;

    /**
     * Minimum limit to play hurry sound.
     */
    private static final long MINIMUM_TO_PLAY_SOUND = 10;

    /**
     * Indication if sound was played.
     */
    private boolean played = false;

    /**
     * Sounds.
     */
    private Sounds sounds;

    /**
     * MyCountDownTimer constructor.
     * @param start start time
     * @param countDownInterval interval
     * @param tv text view
     * @param remain remain string
     * @param postAction callback action
     * @param sounds sounds
     */
    public MyCountDownTimer(long start, long countDownInterval, TextView tv, String remain, IPostAction postAction,
                            Sounds sounds) {
        this.start = start;
        this.interval = countDownInterval;
        this.tv = tv;
        this.remain = remain;
        this.postAction = postAction;
        this.sounds = sounds;
    }

    /**
     * Method for pausing timer.
     */
    public void pause(){
        if(this.cdt != null)
            this.cdt.cancel();
    }

    /**
     * Method for resuming timer.
     */
    public void resume() {
        this.cdt = new CountDownTimer(this.millisUntilFinished != -1 ? this.millisUntilFinished : this.start, this.interval) {
            @Override
            public void onTick(long millisUntilFinished) {
                MyCountDownTimer.this.millisUntilFinished = millisUntilFinished;

                long value = millisUntilFinished / 1000;
                MyCountDownTimer.this.tv.setText(String.format("%s %ds", remain, value));

                if(!played && value < MINIMUM_TO_PLAY_SOUND) {
                    played = true;
                    sounds.playHurry();
                }
            }

            @Override
            public void onFinish() {
                postAction.doAction(null);
            }
        };

        this.cdt.start();
    }
}
